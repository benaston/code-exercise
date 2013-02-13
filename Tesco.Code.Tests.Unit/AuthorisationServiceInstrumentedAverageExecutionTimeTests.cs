// ReSharper disable InconsistentNaming

using System;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tesco.Code.Tests.Unit
{
    [TestFixture]
    public class AuthorisationServiceInstrumentedAverageExecutionTimeTests
    {
        private AverageExecutionTimeDecorator<IAuthorisationService> _decorator;
        private IAuthorisationService _decoratee;
        private ITescoStopwatch _stopwatch;
        private ILogger _logger;

        [SetUp]
        public void SetUp()
        {
            AverageExecutionTimeDecorator<IAuthorisationService>.ResetCounter();
            _decoratee = MockRepository.GenerateMock<IAuthorisationService>();
            _stopwatch = MockRepository.GenerateMock<ITescoStopwatch>();
            _logger = MockRepository.GenerateMock<ILogger>();
            _decorator = new AverageExecutionTimeDecorator<IAuthorisationService>(_decoratee, _stopwatch, 1, _logger);
        }

        [TestCase(0, 1, 0)]
        [TestCase(1, 1, 1)]
        [TestCase(1, 2, 0)]
        [TestCase(2, 3, 0)]
        [TestCase(3, 3, 1)]
        [TestCase(5, 3, 1)]
        [TestCase(6, 3, 2)]
        [TestCase(12, 3, 4)]
        public void Authorise_RequestsPerLogEntrySet_CallsLoggerCorrectNumberOfTimes(int numberOfRequests,
                                                                                     int requestsPerLogEntry,
                                                                                     int expectedNumberOfCallsToLogger)
        {
            //arrange
            _decorator = new AverageExecutionTimeDecorator<IAuthorisationService>(_decoratee, 
                                                                                  _stopwatch, 
                                                                                  requestsPerLogEntry, 
                                                                                  _logger);
            var request = new AuthorisationRequest();

            //act
            for (int x = 0; x < numberOfRequests; x++)
            {
                _decorator.Authorise(request);
            }

            //assert
            _logger.AssertWasCalled(l => l.Log(Arg<string>.Is.Anything), 
                                    options => options.Repeat.Times(expectedNumberOfCallsToLogger));
        }
        
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-2)]
        public void Authorise_RequestsperLogEntryLessThanOne_ExceptionThrown(int requestsPerLogEntry)
        {
            //arrange & act & assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new AverageExecutionTimeDecorator<IAuthorisationService>(_decoratee,
                                                                                  _stopwatch,
                                                                                  requestsPerLogEntry, _logger)); 
        }

        [Test]
        public void Authorise_Always_CallsDecoratedAuthorisationService()
        {
            //arrange
            var request = new AuthorisationRequest();

            //act
            _decorator.Authorise(request);

            //assert
            _decoratee.AssertWasCalled(d => d.Authorise(request));
        }

        [Test]
        public void Authorise_Always_CallsStopwatchStart()
        {
            //arrange
            var request = new AuthorisationRequest();

            //act
            _decorator.Authorise(request);

            //assert
            _stopwatch.AssertWasCalled(s => s.Start());
        }

        [Test]
        public void Authorise_Always_CallsStopwatchStop()
        {
            //arrange
            var request = new AuthorisationRequest();

            //act
            _decorator.Authorise(request);

            //assert
            _stopwatch.AssertWasCalled(s => s.Stop());
        }
    }
}

// ReSharper restore InconsistentNaming