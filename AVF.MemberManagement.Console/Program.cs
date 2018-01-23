using System;
using System.Diagnostics;
using AVF.MemberManagement.Factories;
using AVF.MemberManagement.xUnitIntegrationTests;
using Microsoft.Practices.Unity;
using AVF.MemberManagement.BusinessLogic;

namespace AVF.MemberManagement.Console
{
    internal class Program
    {
        public static IUnityContainer Container;

        public static void Main()
        {
            try
            {
                TraceListener[] listeners = { new TextWriterTraceListener(System.Console.Out) };
                Debug.Listeners.AddRange(listeners);

                var bootstrapper = new Bootstrapper(false);

                bootstrapper.Run();

                Container = bootstrapper.Container;

                DatabaseWrapper m_dbWrapper;
                m_dbWrapper = new DatabaseWrapper();
                System.Console.WriteLine("read database");
                m_dbWrapper.ReadTables(Container).Wait();

                System.Console.Clear();

                var input = 0;
                while (input == 0)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine("1: Generate class prototypes");
                    System.Console.WriteLine("2: Stundensatz-Kalkulator");
                    System.Console.WriteLine("3: Cache Database");
                    System.Console.WriteLine("4: Prüfungsliste");
                    System.Console.WriteLine("5: Graduierungsliste");
                    System.Console.WriteLine("6: Mitgliederbeiträge");
                    System.Console.WriteLine("7: Konsistenzprüfungen");
                    System.Console.WriteLine("8: Trainingsteilnahme");
                    System.Console.WriteLine();
                    System.Console.Write("Please enter number and press ENTER: ");

                    int.TryParse(System.Console.ReadLine(), out input);

                    switch (input)
                    {
                        case 1:
                            new ClassPrototypeGenerator().Main().Wait();
                            break;
                        case 2:
                            new StundensatzKalkulator().Main(m_dbWrapper, 2017).Wait();
                            break;
                        case 3:
                            new JsonFileFactory(Container).RefreshFileCache().Wait();
                            break;
                        case 4:
                            new Pruefungsliste().Main(m_dbWrapper).Wait();
                            break;
                        case 5:
                            new Graduierungsliste().Main(m_dbWrapper).Wait();
                            break;
                        case 6:
                            new Mitgliederbeitraege().Main(m_dbWrapper).Wait();
                            break;
                        case 7:
                            new CheckConsistancy().Main(m_dbWrapper).Wait();
                            break;
                        case 8:
                            Trainingsteilnahme.Report(m_dbWrapper, 2017 );
                            break;
                        default:
                            break;
                    }
                }

                System.Console.WriteLine();
                System.Console.WriteLine("Press any key to exit.");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }

            System.Console.ReadKey();
        }
    }
}
