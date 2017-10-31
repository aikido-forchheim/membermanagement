using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AVF.MemberManagement.StandardLibrary.Factories
{
    public class TableObjectGenerator : ITableObjectGenerator
    {
        public async Task<List<string>> GetTableNames(IPhpCrudApiService phpCrudApiService)
        {
            var json = await phpCrudApiService.GetDataAsync("tables", true);

            var jContainer = (JContainer)JsonConvert.DeserializeObject(json);

            var jArray = (JArray)jContainer.First().First();

            var tablesNames = (from JObject jObject in jArray.Children()
                let tablename = jObject["TABLE_NAME"]
                let tableschema = jObject["TABLE_SCHEMA"]
                where tableschema.ToString() != "information_schema"
                select tablename.ToString()).ToList();
            return tablesNames;
        }

        public async Task<Dictionary<string, string>> GetColumnTypes(IPhpCrudApiService phpCrudApiService, string tablename)
        {
            var json = await phpCrudApiService.GetDataAsync(tablename);

            var jContainer = (JContainer)JsonConvert.DeserializeObject(json);

            var table = jContainer.First().First();

            var columns = table["columns"].Select(column => column.ToString()).ToList();

            var records = table["records"];

            var firstRecord = records[1];

            var columnTypes = columns.ToDictionary<string, string, string>(column => column, column => null);

            for (var index = 0; index < columns.Count; index++)
            {
                var column = columns[index];
                var jToken = firstRecord[index];
                switch (jToken.Type)
                {
                    case JTokenType.None:
                        UnknownMapString(columnTypes, column);
                        break;
                    case JTokenType.Object:
                        UnknownMapString(columnTypes, column);
                        break;
                    case JTokenType.Array:
                        UnknownMapString(columnTypes, column);
                        break;
                    case JTokenType.Constructor:
                        UnknownMapString(columnTypes, column);
                        break;
                    case JTokenType.Property:
                        UnknownMapString(columnTypes, column);
                        break;
                    case JTokenType.Comment:
                        UnknownMapString(columnTypes, column);
                        break;
                    case JTokenType.Integer:
                        columnTypes[column] = "int";
                        break;
                    case JTokenType.Float:
                        UnknownMapString(columnTypes, column);
                        break;
                    case JTokenType.String:
                        RemapString(columnTypes, column, jToken);
                        break;
                    case JTokenType.Boolean:
                        UnknownMapString(columnTypes, column);
                        break;
                    case JTokenType.Null:
                        UnknownMapString(columnTypes, column);
                        break;
                    case JTokenType.Undefined:
                        UnknownMapString(columnTypes, column);
                        break;
                    case JTokenType.Date:
                        UnknownMapString(columnTypes, column);
                        break;
                    case JTokenType.Raw:
                        UnknownMapString(columnTypes, column);
                        break;
                    case JTokenType.Bytes:
                        UnknownMapString(columnTypes, column);
                        break;
                    case JTokenType.Guid:
                        columnTypes[column] = "Guid";
                        break;
                    case JTokenType.Uri:
                        columnTypes[column] = "Uri";
                        break;
                    case JTokenType.TimeSpan:
                        columnTypes[column] = "TimeSpan";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return columnTypes;
        }

        private static void UnknownMapString(IDictionary<string, string> columnTypes, string column)
        {
            columnTypes[column] = "string";
        }

        private static void RemapString(IDictionary<string, string> columnTypes, string column, JToken jToken)
        {
            var tokenString = jToken.ToString();

            if (tokenString.Contains(":") && tokenString.Contains("-"))
            {
                columnTypes[column] = "DateTime";
            }
            else if (tokenString.Contains(":"))
            {
                columnTypes[column] = "TimeSpan";
            }
            else if (tokenString.Contains("-"))
            {
                columnTypes[column] = "DateTime";
            }
            else if (tokenString == "0" || tokenString == "1")
            {
                columnTypes[column] = "bool";
            }
            else
            {
                columnTypes[column] = "string";
            }
        }
    }
}
