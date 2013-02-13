using System;
using System.Threading;

namespace Tesco.Code.Example
{
    /// <summary>
    /// Implementation with a delay to simulate real use.
    /// </summary>
    public class AuthorisationServiceWithDelay : IAuthorisationService
    {
        readonly Random _randomNumberGenerator = new Random();

        public void Authorise(AuthorisationRequest request)
        {
            Thread.Sleep(_randomNumberGenerator.Next(25));
        }
    }
}