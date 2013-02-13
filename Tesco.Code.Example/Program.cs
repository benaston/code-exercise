using System;

namespace Tesco.Code.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = 
                new LogAverageExecutionTimeDecorator<IAuthorisationService>
                    (new AuthorisationServiceWithDelay(),
                     new TescoStopwatch(), 
                     new ConsoleLogger());

            for (int x = 0; x < 500; x++) //500 is a random number large enough to watch the "logging" in progress
            {
                service.Authorise(new AuthorisationRequest());
            }

            Console.WriteLine("Example complete. Press any key to continue...");
            Console.ReadLine();
        }
    }
}
