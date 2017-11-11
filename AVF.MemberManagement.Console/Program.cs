using System;
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
                            new StundensatzKalkulator().Main2().Wait();
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
