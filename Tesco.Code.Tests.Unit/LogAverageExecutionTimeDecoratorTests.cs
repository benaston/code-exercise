// ReSharper disable InconsistentNaming

using System;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tesco.Code.Tests.Unit
{
    /// <summary>
    /// This codebase was developed using TDD. I have left the interaction 
    /// tests in this test fixture to "show my workings". Interaction tests 
    /// can sometimes offer little value beyond the initial help they give 
    /// the developer fleshing out their implementation.
    /// </summary>
    [TestFixture]
    public class LogAverageExecutionTimeDecoratorTests
    {
        private const int MaxRequestsPerEntry = LogAverageExecutionTimeDecorator<IAuthorisationService>.MaximumRequestsPerLogEntry;
        private LogAverageExecutionTimeDecorator<IAuthorisationService> _decorator;
        private IAuthorisationService _decoratee;
        private ITescoStopwatch _stopwatch;
        private ILogger _logger;

        [SetUp]
        public void SetUp()
        {
            LogAverageExecutionTimeDecorator<IAuthorisationService>.ResetRequestCounter();
            _decoratee = MockRepository.GenerateMock<IAuthorisationService>();
            _stopwatch = MockRepository.GenerateMock<ITescoStopwatch>();
            _logger = MockRepository.GenerateMock<ILogger>();
            _decorator = new LogAverageExecutionTimeDecorator<IAuthorisationService>(_decoratee, _stopwatch, _logger, 1);
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
            _decorator = new LogAverageExecutionTimeDecorator<IAuthorisationService>(_decoratee, 
                                                                                  _stopwatch, 
                                                                                  _logger, requestsPerLogEntry);
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
        public void Authorise_RequestsPerLogEntryLessThanOne_ExceptionThrown(int requestsPerLogEntry)
        {
            //arrange & act & assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new LogAverageExecutionTimeDecorator<IAuthorisationService>(_decoratee,
                                                                                  _stopwatch, _logger, requestsPerLogEntry)); 
        }

        [TestCase(MaxRequestsPerEntry + 1)]
        [TestCase(MaxRequestsPerEntry + 2)]
        [TestCase(int.MaxValue)]
        public void Authorise_RequestsPerLogEntryGreaterThan2000_ExceptionThrown(int requestsPerLogEntry)
        {
            //arrange & act & assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new LogAverageExecutionTimeDecorator<IAuthorisationService>(_decoratee,
                                                                                  _stopwatch, _logger, requestsPerLogEntry)); 
        }

        [TestCase(1,2,3, 2000)]
        [TestCase(0,0,0, 0000)]
        [TestCase(1,1,1, 1000)]
        [TestCase(100,100,100, 100000)]
        [TestCase(100,200,600, 300000)]
        public void Authorise_Always_LogsAverageExecutionTime(int timing1, int timing2, int timing3, int expectedAverage)
        {
            //arrange
            var request = new AuthorisationRequest();
            var expectedMessage = string.Format(LogAverageExecutionTimeDecorator<IAuthorisationService>.LogMessageFormat, expectedAverage, "3");
            _decorator = new LogAverageExecutionTimeDecorator<IAuthorisationService>(_decoratee,
                                                                                     _stopwatch,
                                                                                     _logger, 3);

            //act
            _stopwatch.Stub(s => s.ElapsedTime).Return(new TimeSpan(0, 0, 0, timing1)).Repeat.Once();
            _decorator.Authorise(request);
            _stopwatch.Stub(s => s.ElapsedTime).Return(new TimeSpan(0, 0, 0, timing2)).Repeat.Once();
            _decorator.Authorise(request);
            _stopwatch.Stub(s => s.ElapsedTime).Return(new TimeSpan(0, 0, 0, timing3)).Repeat.Once();
            _decorator.Authorise(request);

            //assert
            _logger.AssertWasCalled(d => d.Log(expectedMessage));
        }

        [Test]
        public void Authorise_InvokedOverMaximumRequestsPerLogEntry_ResetsCounterAndContinues()
        {
            //arrange
            var request = new AuthorisationRequest();
            var expectedMessage = string.Format(LogAverageExecutionTimeDecorator<IAuthorisationService>.LogMessageFormat, 1, MaxRequestsPerEntry);
            _stopwatch.Stub(s => s.ElapsedTime).Return(new TimeSpan(0, 0, 0, 0, 1));
            _decorator = new LogAverageExecutionTimeDecorator<IAuthorisationService>(_decoratee,
                                                                                     _stopwatch,
                                                                                     _logger, MaxRequestsPerEntry);

            //act
            for (int x = 0; x <= MaxRequestsPerEntry*3; x++)
            {
                _decorator.Authorise(request);
            }

            //assert
            _logger.AssertWasCalled(l => l.Log(expectedMessage),
                                    options => options.Repeat.Times(3));
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