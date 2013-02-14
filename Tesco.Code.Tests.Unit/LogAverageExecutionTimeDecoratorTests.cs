// ReSharper disable InconsistentNaming

using System;
using System.Threading;
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
        private const int MaxSampleSize = LogAverageExecutionTimeDecorator<IAuthorisationService>.MaximumSampleSize;
        private LogAverageExecutionTimeDecorator<IAuthorisationService> _decorator;
        private IAuthorisationService _decoratee;
        private ITescoStopwatch _stopwatch;
        private ILogger _logger;
        private IClock _clock;
        private readonly DateTime _applicationNow = new DateTime(2000,1,1);
        private const string _logMessageFormat = LogAverageExecutionTimeDecorator<IAuthorisationService>.LogMessageFormat;

        [SetUp]
        public void SetUp()
        {
            LogAverageExecutionTimeDecoratorForTest<IAuthorisationService>.ResetRequestCounter();
            _decoratee = MockRepository.GenerateMock<IAuthorisationService>();
            _stopwatch = MockRepository.GenerateMock<ITescoStopwatch>();
            _logger = MockRepository.GenerateMock<ILogger>();
            _clock = MockRepository.GenerateMock<IClock>();
            _clock.Stub(c => c.ApplicationNow).Return(_applicationNow);
            _decorator = new LogAverageExecutionTimeDecorator<IAuthorisationService>(_decoratee, _stopwatch, _logger, _clock, 1);
        }

        [TestCase(0, 1, 0)]
        [TestCase(1, 1, 1)]
        [TestCase(1, 2, 0)]
        [TestCase(2, 3, 0)]
        [TestCase(3, 3, 1)]
        [TestCase(5, 3, 1)]
        [TestCase(6, 3, 2)]
        [TestCase(12, 3, 4)]
        public void Authorise_SampleSizeSupplied_CallsLoggerCorrectNumberOfTimes(int numberOfRequests,
                                                                                int sampleSize,
                                                                                int expectedNumberOfCallsToLogger)
        {
            //arrange
            _decorator = new LogAverageExecutionTimeDecorator<IAuthorisationService>(_decoratee, 
                                                                                     _stopwatch, 
                                                                                     _logger, 
                                                                                     _clock, 
                                                                                     sampleSize);
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
        public void Authorise_SampleSizeLessThanOne_ExceptionThrown(int sampleSize)
        {
            //arrange & act & assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new LogAverageExecutionTimeDecorator<IAuthorisationService>(_decoratee, 
                    _stopwatch, _logger, _clock, sampleSize)); 
        }

        [TestCase(MaxSampleSize + 1)]
        [TestCase(MaxSampleSize + 2)]
        [TestCase(int.MaxValue)]
        public void Authorise_SampleSizeLargerThanMaximum_ExceptionThrown(int sampleSize)
        {
            //arrange & act & assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new LogAverageExecutionTimeDecorator<IAuthorisationService>(_decoratee, 
                _stopwatch, _logger, _clock, sampleSize)); 
        }

        [TestCase(1,2,3, 2000)]
        [TestCase(0,0,0, 0000)]
        [TestCase(1,1,1, 1000)]
        [TestCase(100,100,100, 100000)]
        [TestCase(100,200,600, 300000)]
        public void Authorise_GivenARunOf3Timings_LogsAverageExecutionTime(int timing1, 
            int timing2, int timing3, int expectedAverage)
        {
            //arrange
            var request = new AuthorisationRequest();
            var expectedMessage = string.Format(_logMessageFormat, _applicationNow.ToString("o"), expectedAverage, "3");
            _decorator = new LogAverageExecutionTimeDecorator<IAuthorisationService>(_decoratee,
                                                                                     _stopwatch,
                                                                                     _logger, _clock, 3);

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
        public void Authorise_InvokedOverSampleSize_ContinuesLogging()
        {
            //arrange
            var request = new AuthorisationRequest();
            var expectedMessage = string.Format(_logMessageFormat, _applicationNow.ToString("o"), 1, MaxSampleSize);
            _stopwatch.Stub(s => s.ElapsedTime).Return(new TimeSpan(0, 0, 0, 0, 1));
            _decorator = new LogAverageExecutionTimeDecorator<IAuthorisationService>(_decoratee, 
                _stopwatch, _logger, _clock, MaxSampleSize);

            //act
            for (int x = 0; x <= MaxSampleSize*3; x++)
            {
                _decorator.Authorise(request);
            }

            //assert
            _logger.AssertWasCalled(l => l.Log(expectedMessage),
                                    options => options.Repeat.Times(3));
        }
        
        /// <summary>
        /// NOTE: slow unit tests like this should be avoided.
        /// </summary>
        [Test]
        public void Authorise_MultipleThreads_LogsExpectedNumberOfTimes()
        {
            //arrange
            var request = new AuthorisationRequest();
            var expectedMessage = string.Format(_logMessageFormat, _applicationNow.ToString("o"), 1, 100);
            _stopwatch.Stub(s => s.ElapsedTime).Return(new TimeSpan(0, 0, 0, 0, 1));
            _decorator = new LogAverageExecutionTimeDecorator<IAuthorisationService>(_decoratee,
                                                                                     _stopwatch,
                                                                                     _logger, _clock);

            var t1 = new Thread(() =>
            {
                for (int x = 0; x <= MaxSampleSize; x++)
                {
                    _decorator.Authorise(request);
                }
            });
            

            var t2 = new Thread(() =>
            {
                for (int x = 0; x <= MaxSampleSize; x++)
                {
                    _decorator.Authorise(request);
                }
            });
            
            var t3 = new Thread(() =>
            {
                for (int x = 0; x <= MaxSampleSize; x++)
                {
                    _decorator.Authorise(request);
                }
            });

            //act
            t1.Start();
            t2.Start();
            t3.Start();
            t1.Join();
            t2.Join();
            t3.Join();

            //assert
            _logger.AssertWasCalled(l => l.Log(expectedMessage),
                                    options => options.Repeat.Times(60));
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