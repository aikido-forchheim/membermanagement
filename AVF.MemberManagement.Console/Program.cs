using System;
using System.Diagnostics;
using System.Linq.Expressions;
using AVF.MemberManagement.Factories;
using AVF.MemberManagement.xUnitIntegrationTests;
using Microsoft.Practices.Unity;

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

                var bootstrapper = new Bootstrapper();

                bootstrapper.Run();

                Container = bootstrapper.Container;

                System.Console.Clear();

                var input = 0;
                while (input == 0)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine("1: Generate class prototypes");
                    System.Console.WriteLine("2: Stundensatz-Kalkulator");
                    System.Console.WriteLine("3: Cache Database");
                    System.Console.WriteLine();
                    System.Console.Write("Please enter number and press ENTER: ");

                    int.TryParse(System.Console.ReadLine(), out input);

                    switch (input)
                    {
                        case 1:
                            new ClassPrototypeGenerator().Main().Wait();
                            break;
                        case 2:
                            new StundensatzKalkulator().Main().Wait();
                            break;
                        case 3:
                            new JsonDumper(Container).Main().Wait();
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
