using System;
using Rhino.Mocks;
using Rhino.Mocks.Exceptions;
using Rhino.Mocks.Interfaces;

namespace PGK.Extensions.RhinoMocks
{
    public static class MethodOptionsExtensions
    {
        /// <summary>
        /// Assigns the first argument of the stub (or expected) call through the <paramref name="assign"/> lambda
        /// received.
        /// <para> An exception is thrown if the method has no argument or the first one is not of the expected type. </para>
        /// </summary>
        /// 
        /// <typeparam name="T"> Type of the mocked object </typeparam>
        /// <typeparam name="TFirstArgument"> Type of the first argument.</typeparam>
        /// <param name="options"></param>
        /// <param name="assign">The lambda that will be called with the first argument value (C#: x => ... / VB: Sub(x) ...) </param>
        public static IMethodOptions<T> AssignFirstArgument<T, TFirstArgument>(this IMethodOptions<T> options, Action<TFirstArgument> assign)
        {
            return options.AssignArgument<T, TFirstArgument>(0, assign);
        }

        /// <summary>
        /// Assigns the first argument of the stub (or expected) call through the <paramref name="assign"/> lambda
        /// received.
        /// <para> An exception is thrown if the method has no argument or the first one is not of the expected type. </para>
        /// </summary>
        /// 
        /// <typeparam name="TFirstArgument"> Type of the first argument.</typeparam>
        /// <param name="options"></param>
        /// <param name="assign">The lambda that will be called with the first argument value (C#: x => ... / VB: Sub(x) ...) </param>
        public static IMethodOptions<object> AssignFirstArgument<TFirstArgument>(this IMethodOptions<object> options, Action<TFirstArgument> assign)
        {
            return options.AssignFirstArgument<Object, TFirstArgument>(assign);
        }

        /// <summary>
        /// Assigns the argument found at the specified <paramref name="index"/> of the stub (or expected) call 
        /// through the <paramref name="assign"/> lambda received.
        /// <para> An exception is thrown if the method doesn't have the specified argument or he is not of the expected type. </para>
        /// </summary>
        /// 
        /// <typeparam name="T"> Type of the mocked object </typeparam>
        /// <typeparam name="TArgument"> Type of the argument.</typeparam>
        /// <param name="options"></param>
        /// <param name="index">The index of the argument we are looking for</param>
        /// <param name="assign">The lambda that will be called with the argument value (C#: x => ... / VB: Sub(x) ...) </param>
        public static IMethodOptions<T> AssignArgument<T, TArgument>(this IMethodOptions<T> options, int index, Action<TArgument> assign)
        {
            return options.WhenCalled(i => assign(GetArgument<TArgument>(i, index)));
        }

        /// <summary>
        /// Assigns the argument found at the specified <paramref name="index"/> of the stub (or expected) call 
        /// through the <paramref name="assign"/> lambda received.
        /// <para> An exception is thrown if the method doesn't have the specified argument or he is not of the expected type. </para>
        /// </summary>
        /// 
        /// <typeparam name="TArgument"> Type of the argument.</typeparam>
        /// <param name="options"></param>
        /// <param name="index">The index of the argument we are looking for</param>
        /// <param name="assign">The lambda that will be called with the argument value (C#: x => ... / VB: Sub(x) ...) </param>
        public static IMethodOptions<object> AssignArgument<TArgument>(this IMethodOptions<object> options, int index, Action<TArgument> assign)
        {
            return options.AssignArgument<object, TArgument>(index, assign);
        }

        private static TArgument GetArgument<TArgument>(MethodInvocation invocation, int index)
        {
            var args = invocation.Arguments;
            if ((args == null || args.Length < index + 1))
            {
                throw new ExpectationViolationException("There is no argument on this method");
            }
            if ((!typeof(TArgument).IsAssignableFrom(args[index].GetType())))
            {
                var msg = "The argument at index {2} is not of expected type (Expected: '{0}' / Actual: '{1}')";

                msg = string.Format(msg, typeof(TArgument).FullName, args[index].GetType().FullName, index);
                throw new ExpectationViolationException(msg);
            }
            return (TArgument)args[index];
        }
    }
}

