using System;
using System.Windows.Threading;

public static class DispatcherExtension
{
    /// <summary>
    /// Invokes the specified <paramref name="action"/> on the given <paramref name="dispatcher"/>.
    /// </summary>
    /// <param name="dispatcher">The dispatcher on which the <paramref name="action"/> executes.</param>
    /// <param name="action">The <see cref="Action"/> to execute.</param>
    /// <param name="priority">The <see cref="DispatcherPriority"/>.  Defaults to <see cref="DispatcherPriority.ApplicationIdle"/></param>
    public static void InvokeAction(this Dispatcher dispatcher, Action action, DispatcherPriority priority)
    {
        if (dispatcher == null)
            throw new ArgumentNullException("dispatcher");
        if (action == null)
            throw new ArgumentNullException("action");
        dispatcher.Invoke(action, priority);
    }

    /// <summary>
    /// Invokes the specified <paramref name="action"/> on the given <paramref name="dispatcher"/>.
    /// </summary>
    /// <typeparam name="T">The type of the argument of the <paramref name="action"/>.</typeparam>
    /// <param name="dispatcher">The dispatcher on which the <paramref name="action"/> executes.</param>
    /// <param name="action">The <see cref="Action{T}"/> to execute.</param>
    /// <param name="arg">The first argument of the action.</param>
    /// <param name="priority">The <see cref="DispatcherPriority"/>.  Defaults to <see cref="DispatcherPriority.ApplicationIdle"/></param>
    public static void InvokeAction<T>(this Dispatcher dispatcher, Action<T> action, T arg,
                                       DispatcherPriority priority = DispatcherPriority.ApplicationIdle)
    {
        if (dispatcher == null)
            throw new ArgumentNullException("dispatcher");
        if (action == null)
            throw new ArgumentNullException("action");
        dispatcher.Invoke(action, priority, arg);
    }

    /// <summary>
    /// Invokes the specified <paramref name="action"/> on the given <paramref name="dispatcher"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument of the <paramref name="action"/>.</typeparam>
    /// <typeparam name="T2">The type of the second argument of the <paramref name="action"/>.</typeparam>
    /// <param name="dispatcher">The dispatcher on which the <paramref name="action"/> executes.</param>
    /// <param name="action">The <see cref="Action{T1,T2}"/> to execute.</param>
    /// <param name="arg1">The first argument of the action.</param>
    /// <param name="arg2">The second argument of the action.</param>
    /// <param name="priority">The <see cref="DispatcherPriority"/>.  Defaults to <see cref="DispatcherPriority.ApplicationIdle"/></param>
    public static void InvokeAction<T1, T2>(this Dispatcher dispatcher, Action<T1, T2> action, T1 arg1, T2 arg2,
                                            DispatcherPriority priority = DispatcherPriority.ApplicationIdle)
    {
        if (dispatcher == null)
            throw new ArgumentNullException("dispatcher");
        if (action == null)
            throw new ArgumentNullException("action");
        dispatcher.Invoke(action, priority, arg1, arg2);
    }

    /// <summary>
    /// Invokes the specified <paramref name="action"/> on the given <paramref name="dispatcher"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument of the <paramref name="action"/>.</typeparam>
    /// <typeparam name="T2">The type of the second argument of the <paramref name="action"/>.</typeparam>
    /// <typeparam name="T3">The type of the third argument of the <paramref name="action"/>.</typeparam>
    /// <param name="dispatcher">The dispatcher on which the <paramref name="action"/> executes.</param>
    /// <param name="action">The <see cref="Action{T1,T2,T3}"/> to execute.</param>
    /// <param name="arg1">The first argument of the action.</param>
    /// <param name="arg2">The second argument of the action.</param>
    /// <param name="arg3">The third argument of the action.</param>
    /// <param name="priority">The <see cref="DispatcherPriority"/>.  Defaults to <see cref="DispatcherPriority.ApplicationIdle"/></param>
    public static void InvokeAction<T1, T2, T3>(this Dispatcher dispatcher, Action<T1, T2, T3> action, T1 arg1,
                                                T2 arg2, T3 arg3,
                                                DispatcherPriority priority = DispatcherPriority.ApplicationIdle)
    {
        if (dispatcher == null)
            throw new ArgumentNullException("dispatcher");
        if (action == null)
            throw new ArgumentNullException("action");
        dispatcher.Invoke(action, priority, arg1, arg2, arg3);
    }

    /// <summary>
    /// Invokes the specified <paramref name="func"/> on the given <paramref name="source"/>.
    /// </summary>
    /// <typeparam name="TResult">Output of the function</typeparam>
    /// <param name="source">Current Source</param>
    /// <param name="func">Func to be invoked </param>
    /// <returns>Returns the invoked Func output</returns>
    public static TResult InvokeAction<TResult>(this DispatcherObject source, Func<TResult> func)
    {
        if (source.Dispatcher.CheckAccess())
            return func();

        return (TResult)source.Dispatcher.Invoke(func);
    }

    /// <summary>
    /// Invokes the specified <paramref name="func"/> on the given <paramref name="source"/>.
    /// </summary>
    /// <typeparam name="TResult">Output of the function</typeparam>
    /// <param name="source">Current Source</param>
    /// <param name="func">Func to be invoked </param>
    /// <returns>Returns the invoked Func output</returns>
    public static TResult InvokeAction<T, TResult>(this T source, Func<T, TResult> func) where T : DispatcherObject
    {
        if (source.Dispatcher.CheckAccess())
            return func(source);

        return (TResult)source.Dispatcher.Invoke(func, source);
    }

    /// <summary>
    /// Invokes the specified <paramref name="func"/> on the given <paramref name="source"/>.
    /// </summary>
    /// <typeparam name="TResult">Output of the function</typeparam>
    /// <param name="source">Current Source</param>
    /// <param name="func">Func to be invoked </param>
    /// <returns>Returns the invoked Func output</returns>
    public static TResult InvokeAction<TSource, T, TResult>(this TSource source, Func<TSource, T, TResult> func,
                                                            T param1) where TSource : DispatcherObject
    {
        if (source.Dispatcher.CheckAccess())
            return func(source, param1);

        return (TResult)source.Dispatcher.Invoke(func, source, param1);
    }

    /// <summary>
    /// Invokes the specified <paramref name="func"/> on the given <paramref name="source"/>.
    /// </summary>
    /// <typeparam name="TResult">Output of the function</typeparam>
    /// <param name="source">Current Source</param>
    /// <param name="func">Func to be invoked </param>
    /// <returns>Returns the invoked Func output</returns>
    public static TResult InvokeAction<TSource, T1, T2, TResult>(this TSource source,
                                                                 Func<TSource, T1, T2, TResult> func, T1 param1,
                                                                 T2 param2) where TSource : DispatcherObject
    {
        if (source.Dispatcher.CheckAccess())
            return func(source, param1, param2);

        return (TResult)source.Dispatcher.Invoke(func, source, param1, param2);
    }

    /// <summary>
    /// Invokes the specified <paramref name="func"/> on the given <paramref name="source"/>.
    /// </summary>
    /// <typeparam name="TResult">Output of the function</typeparam>
    /// <param name="source">Current Source</param>
    /// <param name="func">Func to be invoked </param>
    /// <returns>Returns the invoked Func output</returns>
    public static TResult InvokeAction<TSource, T1, T2, T3, TResult>(this TSource source,
                                                                     Func<TSource, T1, T2, T3, TResult> func,
                                                                     T1 param1, T2 param2, T3 param3)
        where TSource : DispatcherObject
    {
        if (source.Dispatcher.CheckAccess())
            return func(source, param1, param2, param3);

        return (TResult)source.Dispatcher.Invoke(func, source, param1, param2, param3);
    }

    /// <summary>
    /// Invokes the specified <paramref name="func"/> on the given <paramref name="source"/>.
    /// </summary>
    /// <typeparam name="TResult">Output of the function</typeparam>
    /// <param name="source">Current Source</param>
    /// <param name="func">Func to be invoked </param>
    /// <returns>Returns the invoked Func output</returns>
    public static void InvokeAction(this DispatcherObject source, Action func)
    {
        if (source.Dispatcher.CheckAccess())
            func();
        else
            source.Dispatcher.Invoke(func);
    }

    public static void InvokeAction<TSource>(this TSource source, Action<TSource> func)
        where TSource : DispatcherObject
    {
        if (source.Dispatcher.CheckAccess())
            func(source);
        else
            source.Dispatcher.Invoke(func, source);
    }

    public static void InvokeAction<TSource, T1>(this TSource source, Action<TSource, T1> func, T1 param1)
        where TSource : DispatcherObject
    {
        if (source.Dispatcher.CheckAccess())
            func(source, param1);
        else
            source.Dispatcher.Invoke(func, source, param1);
    }

    public static void InvokeAction<TSource, T1, T2>(this TSource source, Action<TSource, T1, T2> func, T1 param1,
                                                     T2 param2) where TSource : DispatcherObject
    {
        if (source.Dispatcher.CheckAccess())
            func(source, param1, param2);
        else
            source.Dispatcher.Invoke(func, source, param1, param2);
    }

    public static void InvokeAction<TSource, T1, T2, T3>(this TSource source, Action<TSource, T1, T2, T3> func,
                                                         T1 param1, T2 param2, T3 param3)
        where TSource : DispatcherObject
    {
        if (source.Dispatcher.CheckAccess())
            func(source, param1, param2, param3);
        else
            source.Dispatcher.Invoke(func, source, param1, param2, param3);
    }

}