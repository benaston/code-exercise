using System;

namespace Tesco.Code.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new LogAverageExecutionTimeDecorator<IAuthorisationService>(new AuthorisationServiceWithDelay(),
                                                                                      new TescoStopwatch(), 
                                                                                      new ConsoleLogger());

            for (int x = 0; x < 2000; x++)
            {
                service.Authorise(new AuthorisationRequest());
            }

            Console.WriteLine("Example complete. Press any key to continue...");
            Console.ReadLine();
        }
    }
}
