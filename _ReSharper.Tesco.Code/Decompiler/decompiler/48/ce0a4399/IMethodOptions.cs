// Type: Rhino.Mocks.Interfaces.IMethodOptions`1
// Assembly: Rhino.Mocks, Version=3.6.0.0, Culture=neutral, PublicKeyToken=0b3305902db7183f
// Assembly location: C:\foo\code-exercise\Tesco.Code.Tests.Unit\bin\Debug\Rhino.Mocks.dll

using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using System;

namespace Rhino.Mocks.Interfaces
{
  /// <summary>
  /// Allows to define what would happen when a method
  ///             is called.
  /// 
  /// </summary>
  public interface IMethodOptions<T>
  {
    /// <summary>
    /// Better syntax to define repeats.
    /// 
    /// </summary>
    IRepeat<T> Repeat { get; }

    /// <summary>
    /// Set the return value for the method.
    /// 
    /// </summary>
    /// <param name="objToReturn">The object the method will return</param>
    /// <returns>
    /// IRepeat that defines how many times the method will return this value
    /// </returns>
    IMethodOptions<T> Return(T objToReturn);

    /// <summary>
    /// Allow to override this return value in the future
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// IRepeat that defines how many times the method will return this value
    /// </returns>
    IMethodOptions<T> TentativeReturn();

    /// <summary>
    /// Throws the specified exception when the method is called.
    /// 
    /// </summary>
    /// <param name="exception">Exception to throw</param>
    IMethodOptions<T> Throw(Exception exception);

    /// <summary>
    /// Ignores the arguments for this method. Any argument will be matched
    ///             againt this method.
    /// 
    /// </summary>
    IMethodOptions<T> IgnoreArguments();

    /// <summary>
    /// Add constraints for the method's arguments.
    /// 
    /// </summary>
    IMethodOptions<T> Constraints(params AbstractConstraint[] constraints);

    /// <summary>
    /// Set a callback method for the last call
    /// 
    /// </summary>
    IMethodOptions<T> Callback(Delegate callback);

    /// <summary>
    /// Set a delegate to be called when the expectation is matched.
    ///             The delegate return value will be returned from the expectation.
    /// 
    /// </summary>
    IMethodOptions<T> Callback(Delegates.Function<bool> callback);

    /// <summary>
    /// Set a delegate to be called when the expectation is matched.
    ///             The delegate return value will be returned from the expectation.
    /// 
    /// </summary>
    IMethodOptions<T> Callback<TArg0>(Delegates.Function<bool, TArg0> callback);

    /// <summary>
    /// Set a delegate to be called when the expectation is matched.
    ///             The delegate return value will be returned from the expectation.
    /// 
    /// </summary>
    IMethodOptions<T> Callback<TArg0, TArg1>(Delegates.Function<bool, TArg0, TArg1> callback);

    /// <summary>
    /// Set a delegate to be called when the expectation is matched.
    ///             The delegate return value will be returned from the expectation.
    /// 
    /// </summary>
    IMethodOptions<T> Callback<TArg0, TArg1, TArg2>(Delegates.Function<bool, TArg0, TArg1, TArg2> callback);

    /// <summary>
    /// Set a delegate to be called when the expectation is matched.
    ///             The delegate return value will be returned from the expectation.
    /// 
    /// </summary>
    IMethodOptions<T> Callback<TArg0, TArg1, TArg2, TArg3>(Delegates.Function<bool, TArg0, TArg1, TArg2, TArg3> callback);

    /// <summary>
    /// Set a delegate to be called when the expectation is matched.
    ///             The delegate return value will be returned from the expectation.
    /// 
    /// </summary>
    IMethodOptions<T> Callback<TArg0, TArg1, TArg2, TArg3, TArg4>(Delegates.Function<bool, TArg0, TArg1, TArg2, TArg3, TArg4> callback);

    /// <summary>
    /// Set a delegate to be called when the expectation is matched.
    ///             The delegate return value will be returned from the expectation.
    /// 
    /// </summary>
    IMethodOptions<T> Callback<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>(Delegates.Function<bool, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5> callback);

    /// <summary>
    /// Set a delegate to be called when the expectation is matched.
    ///             The delegate return value will be returned from the expectation.
    /// 
    /// </summary>
    IMethodOptions<T> Callback<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Delegates.Function<bool, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> callback);

    /// <summary>
    /// Set a delegate to be called when the expectation is matched.
    ///             The delegate return value will be returned from the expectation.
    /// 
    /// </summary>
    IMethodOptions<T> Callback<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Delegates.Function<bool, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> callback);

    /// <summary>
    /// Set a delegate to be called when the expectation is matched.
    ///             The delegate return value will be returned from the expectation.
    /// 
    /// </summary>
    IMethodOptions<T> Callback<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Delegates.Function<bool, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> callback);

    /// <summary>
    /// Set a delegate to be called when the expectation is matched.
    ///             The delegate return value will be returned from the expectation.
    /// 
    /// </summary>
    IMethodOptions<T> Callback<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(Delegates.Function<bool, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> callback);

    /// <summary>
    /// Set a delegate to be called when the expectation is matched.
    ///             The delegate return value will be returned from the expectation.
    /// 
    /// </summary>
    IMethodOptions<T> Do(Delegate action);

    /// <summary>
    /// Set a delegate to be called when the expectation is matched
    ///             and allow to optionally modify the invocation as needed
    /// 
    /// </summary>
    IMethodOptions<T> WhenCalled(System.Action<MethodInvocation> action);

    /// <summary>
    /// Call the original method on the class, bypassing the mocking layers.
    /// 
    /// </summary>
    /// 
    /// <returns/>
    [Obsolete("Use CallOriginalMethod(OriginalCallOptions options) overload to explicitly specify the call options")]
    void CallOriginalMethod();

    /// <summary>
    /// Call the original method on the class, optionally bypassing the mocking layers.
    /// 
    /// </summary>
    /// 
    /// <returns/>
    IMethodOptions<T> CallOriginalMethod(OriginalCallOptions options);

    /// <summary>
    /// Use the property as a simple property, getting/setting the values without
    ///             causing mock expectations.
    /// 
    /// </summary>
    IMethodOptions<T> PropertyBehavior();

    /// <summary>
    /// Expect last (property) call as property setting, ignore the argument given
    /// 
    /// </summary>
    /// 
    /// <returns/>
    IMethodOptions<T> SetPropertyAndIgnoreArgument();

    /// <summary>
    /// Expect last (property) call as property setting with a given argument.
    /// 
    /// </summary>
    /// <param name="argument"/>
    /// <returns/>
    IMethodOptions<T> SetPropertyWithArgument(T argument);

    /// <summary>
    /// Get an event raiser for the last subscribed event.
    /// 
    /// </summary>
    IEventRaiser GetEventRaiser();

    /// <summary>
    /// Set the parameter values for out and ref parameters.
    ///             This is done using zero based indexing, and _ignoring_ any non out/ref parameter.
    /// 
    /// </summary>
    IMethodOptions<T> OutRef(params object[] parameters);

    /// <summary>
    /// Documentation message for the expectation
    /// 
    /// </summary>
    /// <param name="documentationMessage">Message</param>
    IMethodOptions<T> Message(string documentationMessage);
  }
}
