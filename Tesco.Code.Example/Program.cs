using System;
using System.Threading;

namespace Tesco.Code.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var consoleLogger = new ConsoleLogger();
            var authorisationServiceWithDelay = new AuthorisationServiceWithDelay();
            var executionCountDecorator =
                new LogExecutionCountDecorator<IAuthorisationService>
                    (authorisationServiceWithDelay, consoleLogger, new TescoClock(), 10);
            var service = 
                new LogAverageExecutionTimeDecorator<IAuthorisationService>
                    (executionCountDecorator, new TescoStopwatch(), consoleLogger, new TescoClock());

            for (int x = 0; x < 100; x++) //100 is a random number large enough to watch the "logging" in progress
            {
                service.Authorise(new AuthorisationRequest());
                Thread.Sleep(100);
            }

            Console.WriteLine("Example complete. Press any key to continue...");
            Console.ReadLine();
        }
    }
}
