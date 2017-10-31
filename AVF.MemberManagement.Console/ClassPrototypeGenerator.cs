using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.Console
{
    public class ClassPrototypeGenerator
    {
        #region Templates

        //0 = classname 1 = properties
        const string TboClassTemplate = @"using AVF.MemberManagement.StandardLibrary.Interfaces;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tbo
{{
    public class {0} : IIntId
    {{{1}
    }}
}}
";
        //0 = typename 1 = columnname
        const string AdditionalPropertyTemplate = @"

        public {0} {1}
        {{
            get;
            set;
        }}";


        //0 = columnnameOfFirstColumn
        const string IdPropertyTemplate = @"
        public const string PrimaryKey = ""{0}"";

        [JsonProperty(PropertyName = PrimaryKey)]
        public int Id
        {{
            get;
            set;
        }}";

        //0 = tablename 1 = classname
        private const string TblClassTemplate = @"using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tables
{{
    public class {0} : ITable<{1}>
    {{
        public const string TableName = ""{2}"";

        public string Uri {{ get; set; }} = TableName;

        [JsonProperty(PropertyName = TableName)]
        public List<{1}> Rows {{ get; set; }}
    }}
}}
";
        //0 = classname, 1 = tablename
        const string RegisterTemplate = @"
                Container.RegisterType<IProxy<{0}>, Proxy<{1}>>();
                Container.RegisterType<IRepository<{0}>, Repository<{0}>>();
";
        #endregion

        internal async Task Main()
        {
            var sourceCodeBaseDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory?.Parent?.Parent?.Parent?.FullName;

            if (sourceCodeBaseDirectory == null) return;

            var standardLibraryDirectory = Path.Combine(sourceCodeBaseDirectory,
                "AVF.MemberManagement.StandardLibrary");

            var tableDirectory = Path.Combine(standardLibraryDirectory,
                "Tables");

            var tboDirectory = Path.Combine(standardLibraryDirectory,
                "Tbo");

            System.Console.WriteLine(tableDirectory);
            System.Console.WriteLine(tboDirectory);

            var phpCrudApiService = Program.Container.Resolve<IPhpCrudApiService>();
            var tableObjectGenerator = Program.Container.Resolve<ITableObjectGenerator>();

            var tableNames = await tableObjectGenerator.GetTableNames(phpCrudApiService);

            List<string> registrations = new List<string>();

            foreach (var tableName in tableNames)
            {
                var wasPlural = false;

                var tboname = GetTboName(tableName, ref wasPlural);

                if (tboname == null) continue;

                var tblClassName = "Tbl" + RemoveTblPrefix(tableName);

                if (Directory.GetFiles(tboDirectory, tboname + ".cs").Any()) continue;

                System.Console.BackgroundColor = ConsoleColor.Green;
                System.Console.WriteLine(tboname + "  -  " + wasPlural + "  -  " + tblClassName);
                System.Console.BackgroundColor = ConsoleColor.Black;

                if (!wasPlural)
                {
                    WriteLine($"The name was no plural, is the tblClassName correct?");

                    SendKeys.SendWait(tblClassName); //only works with windows
                    Clipboard.SetText(tblClassName); //does not work with mac either

                    tblClassName = System.Console.ReadLine();
                }

                string properties = GetProperties(await tableObjectGenerator.GetColumnTypes(phpCrudApiService, tableName));

                var tboContent = GenerateTbo(tboname, properties);

                var tblContent = GenerateTbl(tblClassName, tboname, tableName);

                var registration = GenerateRegistration(tboname, tblClassName);
                registrations.Add(registration);

                var tboPath = Path.Combine(tboDirectory, tboname + ".cs");
                var tblPath = Path.Combine(tableDirectory, tblClassName + ".cs");

                WriteLine(tboPath);
                WriteLine(tblPath);

                File.WriteAllText(tboPath, tboContent, Encoding.UTF8);
                File.WriteAllText(tblPath, tblContent, Encoding.UTF8);
            }

            string registrationsString = string.Join(Environment.NewLine, registrations);
            WriteLine(registrationsString);

            File.WriteAllText(Path.Combine(tboDirectory, "reg" + DateTime.Now.Ticks + ".txt"), registrationsString, Encoding.UTF8);
        }

        private string GenerateRegistration(string tboname, string tblClassName)
        {
            var result = string.Format(RegisterTemplate, tboname, tblClassName);

            System.Console.WriteLine(result);

            return result;
        }

        private string GetProperties(Dictionary<string, string> dictionary)
        {
            int columnid = 0;
            StringBuilder sb = new StringBuilder();
            foreach (var column in dictionary)
            {
                if (columnid == 0)
                {
                    var idproperty = string.Format(IdPropertyTemplate, column.Key);
                    sb.Append(idproperty);
                }
                else
                {
                    var addproperty = string.Format(AdditionalPropertyTemplate, column.Value, column.Key);
                    sb.Append(addproperty);
                }

                columnid++;
            }

            string allProperties = sb.ToString();

            return allProperties;
        }

        private static string GenerateTbl(string tblClassName, string tboname, string realTableName)
        {
            var result = string.Format(TblClassTemplate, tblClassName, tboname, realTableName);

            System.Console.WriteLine(result);

            return result;
        }

        private string GenerateTbo(string tboname, string properties)
        {
            var result = string.Format(TboClassTemplate, tboname, properties);

            WriteLine(result);

            return result;
        }

        private static void WriteLine(string s)
        {
            System.Console.WriteLine(s);
        }

        private static string GetTboName(string tableName, ref bool wasPlural)
        {
            var tboName = RemoveTblPrefix(tableName);

            if (tableName.EndsWith("en") || tableName.EndsWith("ge"))
            {
                tboName = tboName?.Substring(0, tboName.Length - 1);
                wasPlural = true;
            }

            if (tableName.EndsWith("er"))
            {
                tboName = tboName?.Substring(0, tboName.Length - 2);
                wasPlural = true;
            }

            return tboName;
        }

        private static string RemoveTblPrefix(string tableName)
        {
            string tboName = null;

            if (tableName.StartsWith("tbl"))
            {
                tboName = tableName.Substring(3);
            }
            else if (tableName.StartsWith("tb"))
            {
                tboName = tableName.Substring(2);
            }

            return tboName;
        }
    }
}
