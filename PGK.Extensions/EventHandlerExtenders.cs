using System;
using System.ComponentModel;


public static class EventHandlerExtenders
{
    /// <summary>
    /// Raises an event of any type that implements the standard event signature 
    /// (object sender, :EventArgs e) on the current thread.
    /// </summary>
    /// <remarks>
    /// Events are raised in a thread-safe manner, but are raised on the current thread.
    /// </remarks>
    /// <example>
    ///  //---------------------------------------------------------
    ///   public class MyEventArgs : EventArgs
    ///    {
    ///        private string msg;
    ///
    ///        public MyEventArgs(string messageData)
    ///        {
    ///            msg = messageData;
    ///       }
    ///        public string Message
    ///        {
    ///            get { return msg; }
    ///            set { msg = value; }
    ///        }
    ///    }
    ///  //--------------------------------------------------------- 
    ///   public class ClassWithACustomEvent
    ///   {
    ///     // Declare an event of delegate type EventHandler of 
    ///     // MyEventArgs.
    ///
    ///       public event EventHandler<MyEventArgs> SampleEvent;
    ///
    ///       public void OnDemoEvent(MyEventArgs e)
    ///       {
    ///           // Raise the event on the current thread
    ///           SampleEvent.RaiseEvent(this, e);
    ///       }
    ///
    ///       public void OnDemoEventUIThread(MyEventArgs e)
    ///        {
    ///           // Raise the event on the subscribers UI thread, if possible
    ///          SampleEvent.RaiseEventOnUIThread(this, e);
    ///     }
    ///   }
    ///   //---------------------------------------------------------
    ///   public class Sample
    ///   {
    ///     public static void Main()
    ///      {
    ///         ClassWithACustomEvent theClass = new ClassWithACustomEvent();
    ///         theClass.SampleEvent += new EventHandler<MyEventArgs>(SampleEventHandler);
    ///         theClass.OnDemoEvent(new MyEventArgs("Hey there, Bruce!"));
    ///         theClass.OnDemoEvent(new MyEventArgs("How are you today?"));
    ///         theClass.OnDemoEventUIThread(new MyEventArgs("I'm pretty good."));
    ///         theClass.OnDemoEventUIThread(new MyEventArgs("Thanks for asking!"));
    ///      }
    ///
    ///       private static void SampleEventHandler(object src, MyEventArgs e)
    ///      {
    ///         Console.WriteLine(e.Message);
    ///     }
    ///   }
    ///   //---------------------------------------------------------
    ///   /*
    ///   This example produces the following results:
    ///
    ///   Hey there, JT!
    ///   How are you today?
    ///   I'm pretty good.
    ///   Thanks for asking!
    ///
    ///   */
    /// </code>
    /// </example>
    /// <typeparam name="TEventArgs">The type of the EventHandler used to raise the event.</typeparam>
    /// <param name="eventHandler">The EventHandler instance use to raise the event.</param>
    /// <param name="sender">The sender object instance to pass to subscribers.</param>
    /// <param name="e">The EventArgs (or derivative) to pass to subscribers.</param>
    public static void RaiseEvent<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender,
                                              TEventArgs e)
        where TEventArgs : EventArgs
    {
        if (eventHandler != null)
        {
            eventHandler(sender, e);
        }
    }

    /// <summary>
    /// Raises an event of any type that implements the standard event signature 
    /// (object sender, :EventArgs e) on the event subscribers UI thread if possible.
    /// </summary>
    /// <remarks>
    /// Events are raised in a thread-safe manner, and will be raised on the subscribers thread
    /// in cases where subscribers implement the <see cref="ISynchronizeInvoke"/> interface 
    /// (i.e. WindowsForms controls), otherwise they will be raised on the current thread.
    /// </remarks>
    /// <example>
    /// <code> 
    /// 
    ///  //---------------------------------------------------------
    ///   public class MyEventArgs : EventArgs
    ///    {
    ///        private string msg;
    ///
    ///        public MyEventArgs(string messageData)
    ///        {
    ///            msg = messageData;
    ///       }
    ///        public string Message
    ///        {
    ///            get { return msg; }
    ///            set { msg = value; }
    ///        }
    ///    }
    ///  //--------------------------------------------------------- 
    ///   public class ClassWithACustomEvent
    ///   {
    ///     // Declare an event of delegate type EventHandler of 
    ///     // MyEventArgs.
    ///
    ///       public event EventHandler<MyEventArgs> SampleEvent;
    ///
    ///       public void OnDemoEvent(MyEventArgs e)
    ///       {
    ///           // Raise the event on the current thread
    ///           SampleEvent.RaiseEvent(this, e);
    ///       }
    ///
    ///       public void OnDemoEventUIThread(MyEventArgs e)
    ///        {
    ///           // Raise the event on the subscribers UI thread, if possible
    ///          SampleEvent.RaiseEventOnUIThread(this, e);
    ///     }
    ///   }
    ///   //---------------------------------------------------------
    ///   public class Sample
    ///   {
    ///     public static void Main()
    ///      {
    ///         ClassWithACustomEvent theClass = new ClassWithACustomEvent();
    ///         theClass.SampleEvent += new EventHandler<MyEventArgs>(SampleEventHandler);
    ///         theClass.OnDemoEvent(new MyEventArgs("Hey there, Bruce!"));
    ///         theClass.OnDemoEvent(new MyEventArgs("How are you today?"));
    ///         theClass.OnDemoEventUIThread(new MyEventArgs("I'm pretty good."));
    ///         theClass.OnDemoEventUIThread(new MyEventArgs("Thanks for asking!"));
    ///      }
    ///
    ///       private static void SampleEventHandler(object src, MyEventArgs e)
    ///      {
    ///         Console.WriteLine(e.Message);
    ///     }
    ///   }
    ///   //---------------------------------------------------------
    ///   /*
    ///   This example produces the following results:
    ///
    ///   Hey there, JT!
    ///   How are you today?
    ///   I'm pretty good.
    ///   Thanks for asking!
    ///
    ///   */
    /// </code>
    /// </example>
    /// <typeparam name="TEventArgs">The type of the EventHandler used to raise the event.</typeparam>
    /// <param name="eventHandler">The EventHandler instance use to raise the event.</param>
    /// <param name="sender">The sender object instance to pass to subscribers.</param>
    /// <param name="e">The EventArgs (or derivative) to pass to subscribers.</param>
    public static void RaiseEventOnUIThread<TEventArgs>(
        this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs e)
        where TEventArgs : EventArgs
    {
        if (eventHandler != null)
        {
            foreach (EventHandler<TEventArgs> singleCast in eventHandler.GetInvocationList())
            {
                var syncInvoke = singleCast.Target as ISynchronizeInvoke;
                if (syncInvoke != null && syncInvoke.InvokeRequired)
                {
                    // Invoke the event on the event subscribers thread
                    syncInvoke.Invoke(eventHandler, new[] { sender, e });
                }
                else
                {
                    // Raise the event on this thread
                    singleCast(sender, e);
                }
            }
        }
    }
}
