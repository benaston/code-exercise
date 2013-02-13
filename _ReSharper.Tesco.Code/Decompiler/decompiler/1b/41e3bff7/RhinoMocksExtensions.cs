// Type: Rhino.Mocks.RhinoMocksExtensions
// Assembly: Rhino.Mocks, Version=3.6.0.0, Culture=neutral, PublicKeyToken=0b3305902db7183f
// Assembly location: C:\foo\code-exercise\Tesco.Code.Tests.Unit\bin\Debug\Rhino.Mocks.dll

using Rhino.Mocks.Exceptions;
using Rhino.Mocks.Generated;
using Rhino.Mocks.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhino.Mocks
{
  /// <summary>
  /// A set of extension methods that adds Arrange Act Assert mode to Rhino Mocks
  /// 
  /// </summary>
  public static class RhinoMocksExtensions
  {
    /// <summary>
    /// Create an expectation on this mock for this action to occur
    /// 
    /// </summary>
    /// <typeparam name="T"/><param name="mock">The mock.</param><param name="action">The action.</param>
    /// <returns/>
    public static IMethodOptions<RhinoMocksExtensions.VoidType> Expect<T>(this T mock, System.Action<T> action) where T : class
    {
      return RhinoMocksExtensions.Expect<T, RhinoMocksExtensions.VoidType>(mock, (Function<T, RhinoMocksExtensions.VoidType>) (t =>
      {
        action(t);
        return (RhinoMocksExtensions.VoidType) null;
      }));
    }

    /// <summary>
    /// Reset all expectations on this mock object
    /// 
    /// </summary>
    /// <typeparam name="T"/><param name="mock">The mock.</param>
    public static void BackToRecord<T>(this T mock)
    {
      RhinoMocksExtensions.BackToRecord<T>(mock, BackToRecordOptions.All);
    }

    /// <summary>
    /// Reset the selected expectation on this mock object
    /// 
    /// </summary>
    /// <typeparam name="T"/><param name="mock">The mock.</param><param name="options">The options to reset the expectations on this mock.</param>
    public static void BackToRecord<T>(this T mock, BackToRecordOptions options)
    {
      MockRepository.GetMockedObject((object) mock).Repository.BackToRecord((object) mock, options);
    }

    /// <summary>
    /// Cause the mock state to change to replay, any further call is compared to the
    ///             ones that were called in the record state.
    /// 
    /// </summary>
    /// <param name="mock">the mocked object to move to replay state</param>
    public static void Replay<T>(this T mock)
    {
      IMockedObject mockedObject = MockRepository.GetMockedObject((object) mock);
      MockRepository repository = mockedObject.Repository;
      if (repository.IsInReplayMode((object) mock))
        return;
      repository.Replay((object) mockedObject);
    }

    /// <summary>
    /// Gets the mock repository for this specificied mock object
    /// 
    /// </summary>
    /// <typeparam name="T"/><param name="mock">The mock.</param>
    /// <returns/>
    public static MockRepository GetMockRepository<T>(this T mock)
    {
      return MockRepository.GetMockedObject((object) mock).Repository;
    }

    /// <summary>
    /// Create an expectation on this mock for this action to occur
    /// 
    /// </summary>
    /// <typeparam name="T"/><typeparam name="R"/><param name="mock">The mock.</param><param name="action">The action.</param>
    /// <returns/>
    public static IMethodOptions<R> Expect<T, R>(this T mock, Function<T, R> action) where T : class
    {
      if ((object) mock == null)
        throw new ArgumentNullException("mock", "You cannot mock a null instance");
      MockRepository repository = MockRepository.GetMockedObject((object) mock).Repository;
      bool flag = repository.IsInReplayMode((object) mock);
      repository.BackToRecord((object) mock, BackToRecordOptions.None);
      R r = action(mock);
      IMethodOptions<R> options = LastCall.GetOptions<R>();
      options.TentativeReturn();
      if (flag)
        repository.ReplayCore((object) mock, false);
      return options;
    }

    /// <summary>
    /// Tell the mock object to perform a certain action when a matching
    ///             method is called.
    ///             Does not create an expectation for this method.
    /// 
    /// </summary>
    /// <typeparam name="T"/><param name="mock">The mock.</param><param name="action">The action.</param>
    /// <returns/>
    public static IMethodOptions<object> Stub<T>(this T mock, System.Action<T> action) where T : class
    {
      return RhinoMocksExtensions.Stub<T, object>(mock, (Function<T, object>) (t =>
      {
        action(t);
        return (object) null;
      }));
    }

    /// <summary>
    /// Tell the mock object to perform a certain action when a matching
    ///             method is called.
    ///             Does not create an expectation for this method.
    /// 
    /// </summary>
    /// <typeparam name="T"/><typeparam name="R"/><param name="mock">The mock.</param><param name="action">The action.</param>
    /// <returns/>
    public static IMethodOptions<R> Stub<T, R>(this T mock, Function<T, R> action) where T : class
    {
      return RhinoMocksExtensions.Expect<T, R>(mock, action).Repeat.Times(0, int.MaxValue);
    }

    /// <summary>
    /// Gets the arguments for calls made on this mock object and the method that was called
    ///             in the action.
    /// 
    /// </summary>
    /// <typeparam name="T"/><param name="mock">The mock.</param><param name="action">The action.</param>
    /// <returns/>
    /// 
    /// <example>
    /// Here we will get all the arguments for all the calls made to DoSomething(int)
    /// 
    /// <code>
    /// var argsForCalls = foo54.GetArgumentsForCallsMadeOn(x =&gt; x.DoSomething(0))
    /// 
    /// </code>
    /// 
    /// </example>
    public static IList<object[]> GetArgumentsForCallsMadeOn<T>(this T mock, System.Action<T> action)
    {
      return RhinoMocksExtensions.GetArgumentsForCallsMadeOn<T>(mock, action, new System.Action<IMethodOptions<object>>(RhinoMocksExtensions.DefaultConstraintSetup));
    }

    /// <summary>
    /// Gets the arguments for calls made on this mock object and the method that was called
    ///             in the action and matches the given constraints
    /// 
    /// </summary>
    /// <typeparam name="T"/><param name="mock">The mock.</param><param name="action">The action.</param><param name="setupConstraints">The setup constraints.</param>
    /// <returns/>
    /// 
    /// <example>
    /// Here we will get all the arguments for all the calls made to DoSomething(int)
    /// 
    /// <code>
    /// var argsForCalls = foo54.GetArgumentsForCallsMadeOn(x =&gt; x.DoSomething(0))
    /// 
    /// </code>
    /// 
    /// </example>
    public static IList<object[]> GetArgumentsForCallsMadeOn<T>(this T mock, System.Action<T> action, System.Action<IMethodOptions<object>> setupConstraints)
    {
      return RhinoMocksExtensions.GetExpectationsToVerify<T>(mock, action, setupConstraints).ArgumentsForAllCalls;
    }

    /// <summary>
    /// Asserts that a particular method was called on this mock object
    /// 
    /// </summary>
    /// <typeparam name="T"/><param name="mock">The mock.</param><param name="action">The action.</param>
    public static void AssertWasCalled<T>(this T mock, System.Action<T> action)
    {
      RhinoMocksExtensions.AssertWasCalled<T>(mock, action, new System.Action<IMethodOptions<object>>(RhinoMocksExtensions.DefaultConstraintSetup));
    }

    private static void DefaultConstraintSetup(IMethodOptions<object> options)
    {
    }

    /// <summary>
    /// Asserts that a particular method was called on this mock object that match
    ///             a particular constraint set.
    /// 
    /// </summary>
    /// <typeparam name="T"/><param name="mock">The mock.</param><param name="action">The action.</param><param name="setupConstraints">The setup constraints.</param>
    public static void AssertWasCalled<T>(this T mock, System.Action<T> action, System.Action<IMethodOptions<object>> setupConstraints)
    {
      ExpectationVerificationInformation expectationsToVerify = RhinoMocksExtensions.GetExpectationsToVerify<T>(mock, action, setupConstraints);
      foreach (object[] args in (IEnumerable<object[]>) expectationsToVerify.ArgumentsForAllCalls)
      {
        if (expectationsToVerify.Expected.IsExpected(args))
          expectationsToVerify.Expected.AddActualCall();
      }
      if (!expectationsToVerify.Expected.ExpectationSatisfied)
        throw new ExpectationViolationException(expectationsToVerify.Expected.BuildVerificationFailureMessage());
    }

    /// <summary>
    /// Asserts that a particular method was called on this mock object that match
    ///             a particular constraint set.
    /// 
    /// </summary>
    /// <typeparam name="T"/><param name="mock">The mock.</param><param name="action">The action.</param>
    public static void AssertWasCalled<T>(this T mock, Func<T, object> action)
    {
      object obj;
      System.Action<T> action1 = (System.Action<T>) (t => obj = action(t));
      RhinoMocksExtensions.AssertWasCalled<T>(mock, action1, new System.Action<IMethodOptions<object>>(RhinoMocksExtensions.DefaultConstraintSetup));
    }

    /// <summary>
    /// Asserts that a particular method was called on this mock object that match
    ///             a particular constraint set.
    /// 
    /// </summary>
    /// <typeparam name="T"/><param name="mock">The mock.</param><param name="action">The action.</param><param name="setupConstraints">The setup constraints.</param>
    public static void AssertWasCalled<T>(this T mock, Func<T, object> action, System.Action<IMethodOptions<object>> setupConstraints)
    {
      object obj;
      System.Action<T> action1 = (System.Action<T>) (t => obj = action(t));
      RhinoMocksExtensions.AssertWasCalled<T>(mock, action1, setupConstraints);
    }

    /// <summary>
    /// Asserts that a particular method was NOT called on this mock object
    /// 
    /// </summary>
    /// <typeparam name="T"/><param name="mock">The mock.</param><param name="action">The action.</param>
    public static void AssertWasNotCalled<T>(this T mock, System.Action<T> action)
    {
      RhinoMocksExtensions.AssertWasNotCalled<T>(mock, action, new System.Action<IMethodOptions<object>>(RhinoMocksExtensions.DefaultConstraintSetup));
    }

    /// <summary>
    /// Asserts that a particular method was NOT called on this mock object that match
    ///             a particular constraint set.
    /// 
    /// </summary>
    /// <typeparam name="T"/><param name="mock">The mock.</param><param name="action">The action.</param><param name="setupConstraints">The setup constraints.</param>
    public static void AssertWasNotCalled<T>(this T mock, System.Action<T> action, System.Action<IMethodOptions<object>> setupConstraints)
    {
      ExpectationVerificationInformation expectationsToVerify = RhinoMocksExtensions.GetExpectationsToVerify<T>(mock, action, setupConstraints);
      foreach (object[] args in (IEnumerable<object[]>) expectationsToVerify.ArgumentsForAllCalls)
      {
        if (expectationsToVerify.Expected.IsExpected(args))
          throw new ExpectationViolationException("Expected that " + expectationsToVerify.Expected.ErrorMessage + " would not be called, but it was found on the actual calls made on the mocked object.");
      }
    }

    /// <summary>
    /// Asserts that a particular method was NOT called on this mock object
    /// 
    /// </summary>
    /// <typeparam name="T"/><param name="mock">The mock.</param><param name="action">The action.</param>
    public static void AssertWasNotCalled<T>(this T mock, Func<T, object> action)
    {
      object obj;
      System.Action<T> action1 = (System.Action<T>) (t => obj = action(t));
      RhinoMocksExtensions.AssertWasNotCalled<T>(mock, action1, new System.Action<IMethodOptions<object>>(RhinoMocksExtensions.DefaultConstraintSetup));
    }

    /// <summary>
    /// Asserts that a particular method was NOT called on this mock object
    /// 
    /// </summary>
    /// <typeparam name="T"/><param name="mock">The mock.</param><param name="action">The action.</param><param name="setupConstraints">The setup constraints.</param>
    public static void AssertWasNotCalled<T>(this T mock, Func<T, object> action, System.Action<IMethodOptions<object>> setupConstraints)
    {
      object obj;
      System.Action<T> action1 = (System.Action<T>) (t => obj = action(t));
      RhinoMocksExtensions.AssertWasNotCalled<T>(mock, action1, setupConstraints);
    }

    private static ExpectationVerificationInformation GetExpectationsToVerify<T>(T mock, System.Action<T> action, System.Action<IMethodOptions<object>> setupConstraints)
    {
      IMockedObject mockedObject = MockRepository.GetMockedObject((object) mock);
      MockRepository repository = mockedObject.Repository;
      if (!repository.IsInReplayMode((object) mockedObject))
        throw new InvalidOperationException("Cannot assert on an object that is not in replay mode. Did you forget to call ReplayAll() ?");
      T mockToRecordExpectation = (T) repository.DynamicMock(RhinoMocksExtensions.FindAppropriteType<T>(mockedObject), mockedObject.ConstructorArguments);
      action(mockToRecordExpectation);
      RhinoMocksExtensions.AssertExactlySingleExpectaton<T>(repository, mockToRecordExpectation);
      IMethodOptions<object> methodOptions = repository.LastMethodCall<object>((object) mockToRecordExpectation);
      methodOptions.TentativeReturn();
      if (setupConstraints != null)
        setupConstraints(methodOptions);
      ExpectationsList expectationsForProxy = repository.Replayer.GetAllExpectationsForProxy((object) mockToRecordExpectation);
      if (expectationsForProxy.Count == 0)
        throw new InvalidOperationException("The expectation was removed from the waiting expectations list, did you call Repeat.Any() ? This is not supported in AssertWasCalled()");
      IExpectation expectation = expectationsForProxy[0];
      ICollection<object[]> callArgumentsFor = mockedObject.GetCallArgumentsFor(expectation.Method);
      return new ExpectationVerificationInformation()
      {
        ArgumentsForAllCalls = (IList<object[]>) new List<object[]>((IEnumerable<object[]>) callArgumentsFor),
        Expected = expectation
      };
    }

    /// <summary>
    /// Finds the approprite implementation type of this item.
    ///             This is the class or an interface outside of the rhino mocks.
    /// 
    /// </summary>
    /// <param name="mockedObj">The mocked obj.</param>
    /// <returns/>
    private static Type FindAppropriteType<T>(IMockedObject mockedObj)
    {
      foreach (Type c in mockedObj.ImplementedTypes)
      {
        if (c.IsClass && typeof (T).IsAssignableFrom(c))
          return c;
      }
      foreach (Type c in mockedObj.ImplementedTypes)
      {
        if (c.Assembly != typeof (IMockedObject).Assembly && typeof (T).IsAssignableFrom(c))
          return c;
      }
      return mockedObj.ImplementedTypes[0];
    }

    /// <summary>
    /// Verifies all expectations on this mock object
    /// 
    /// </summary>
    /// <param name="mockObject">The mock object.</param>
    public static void VerifyAllExpectations(this object mockObject)
    {
      IMockedObject mockedObject = MockRepository.GetMockedObject(mockObject);
      mockedObject.Repository.Verify((object) mockedObject);
    }

    /// <summary>
    /// Gets the event raiser for the event that was called in the action passed
    /// 
    /// </summary>
    /// <typeparam name="TEventSource">The type of the event source.</typeparam><param name="mockObject">The mock object.</param><param name="eventSubscription">The event subscription.</param>
    /// <returns/>
    public static IEventRaiser GetEventRaiser<TEventSource>(this TEventSource mockObject, System.Action<TEventSource> eventSubscription) where TEventSource : class
    {
      return RhinoMocksExtensions.Stub<TEventSource>(mockObject, eventSubscription).IgnoreArguments().GetEventRaiser();
    }

    /// <summary>
    /// Raise the specified event using the passed arguments.
    ///             The even is extracted from the passed labmda
    /// 
    /// </summary>
    /// <typeparam name="TEventSource">The type of the event source.</typeparam><param name="mockObject">The mock object.</param><param name="eventSubscription">The event subscription.</param><param name="sender">The sender.</param><param name="args">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
    public static void Raise<TEventSource>(this TEventSource mockObject, System.Action<TEventSource> eventSubscription, object sender, EventArgs args) where TEventSource : class
    {
      RhinoMocksExtensions.GetEventRaiser<TEventSource>(mockObject, eventSubscription).Raise(sender, args);
    }

    /// <summary>
    /// Raise the specified event using the passed arguments.
    ///             The even is extracted from the passed labmda
    /// 
    /// </summary>
    /// <typeparam name="TEventSource">The type of the event source.</typeparam><param name="mockObject">The mock object.</param><param name="eventSubscription">The event subscription.</param><param name="args">The args.</param>
    public static void Raise<TEventSource>(this TEventSource mockObject, System.Action<TEventSource> eventSubscription, params object[] args) where TEventSource : class
    {
      RhinoMocksExtensions.GetEventRaiser<TEventSource>(mockObject, eventSubscription).Raise(args);
    }

    /// <summary>
    /// TODO: Make this better!  It currently breaks down when mocking classes or
    ///             ABC's that call other virtual methods which are getting intercepted too.  I wish
    ///             we could just walk Expression{Action{Action{T}} to assert only a single
    ///             method is being made.
    /// 
    ///             The workaround is to not call foo.AssertWasCalled .. rather foo.VerifyAllExpectations()
    /// </summary>
    /// <typeparam name="T">The type of mock object</typeparam><param name="mocks">The mock repository</param><param name="mockToRecordExpectation">The actual mock object to assert expectations on.</param>
    private static void AssertExactlySingleExpectaton<T>(MockRepository mocks, T mockToRecordExpectation)
    {
      if (mocks.Replayer.GetAllExpectationsForProxy((object) mockToRecordExpectation).Count == 0)
        throw new InvalidOperationException("No expectations were setup to be verified, ensure that the method call in the action is a virtual (C#) / overridable (VB.Net) method call");
      if (mocks.Replayer.GetAllExpectationsForProxy((object) mockToRecordExpectation).Count > 1)
        throw new InvalidOperationException("You can only use a single expectation on AssertWasCalled(), use separate calls to AssertWasCalled() if you want to verify several expectations");
    }

    /// <summary>
    /// Fake type that disallow creating it.
    ///             Should have been System.Type, but we can't use it.
    /// 
    /// </summary>
    public class VoidType
    {
      private VoidType()
      {
      }
    }
  }
}
