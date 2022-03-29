using System;
using System.Windows;
using System.Windows.Threading;

namespace MarsUI
{
    //public static class DispatchService
    //{
    //    public static void Invoke(Action action)
    //    {
    //        var dispatchObject = Application.Current.Dispatcher;
    //        if (dispatchObject == null || dispatchObject.CheckAccess())
    //            action();
    //        else
    //            dispatchObject.Invoke(action);
    //    }
    //    public static void BeginInvokeThread(Dispatcher frameContentDispatcher, ThreadStart action, DispatcherPriority priority)
    //    {
    //        var dispatchObject = frameContentDispatcher; //Application.Current.Dispatcher;
    //        dispatchObject?.BeginInvoke(action, priority);
    //    }
    //    public static DispatcherOperation BeginInvoke(Action action, DispatcherPriority priority)
    //    {
    //        var dispatchObject = Application.Current.Dispatcher;

    //        if (dispatchObject != null)
    //            return dispatchObject.BeginInvoke(action, priority);

    //        action();
    //        return null;
    //    }
    //    public static DispatcherOperation BeginInvoke(Dispatcher dispatcher, Action action, DispatcherPriority priority)
    //    {
    //        var dispatchObject = dispatcher;

    //        if (dispatchObject != null)
    //            return dispatchObject.BeginInvoke(action, priority);

    //        action();
    //        return null;
    //    }
    //    public static DispatcherOperation BeginInvoke(Action action)
    //    {
    //        var dispatchObject = Application.Current.Dispatcher;

    //        if (dispatchObject != null)
    //            return dispatchObject.BeginInvoke(action);

    //        action();
    //        return null;
    //    }
    //}

    public static class DispatchService
    {
        public static Dispatcher UiDispatcher { get; private set; }

        /// <summary>
        /// This method should be called once on the UI thread to ensure that
        /// the property is initialized.
        /// <para>In WPF, call this method on the static App() constructor.</para>
        /// </summary>
        public static void Initialize()
        {
            if (UiDispatcher != null && UiDispatcher.Thread.IsAlive)
                return;

            UiDispatcher = Dispatcher.CurrentDispatcher;
        }

        /// <summary>
        /// Executes an action on the UI thread.
        /// The action will be enqueued on the UI thread's
        /// dispatcher and executed asynchronously.
        /// </summary>
        /// <param name="action">The action will
        /// be enqueued on the UI thread's dispatcher
        /// and executed asynchronously..
        /// </param>
        public static void BeginInvokeOnUi(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            UiDispatcher.InvokeAsync(action, DispatcherPriority.Input);
        }

        /// <summary>
        /// Executes an action on the UI thread. The action will be enqueued
        /// on the UI thread's dispatcher and executed synchronously.
        /// </summary>
        /// <param name="action">
        /// The action that will be executed on the UI thread synchronously.
        /// </param>
        public static void InvokeOnUi(Action action, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            try
            {
                UiDispatcher.Invoke(action, priority);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Executes an action on the UI thread. The action will be enqueued on the
        /// UI thread's dispatcher with the specified priority and executed asynchronously.
        /// </summary>
        /// <param name="action">
        /// The action that will be executed on the UI thread.</param>
        /// <param name="priority"></param>

        public static DispatcherOperation InvokeOnUiAsync(Action action, DispatcherPriority priority)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            return UiDispatcher.InvokeAsync(action, priority);
        }
        public static DispatcherOperation BeginInvoke(Action action)
        {
            var dispatchObject = Application.Current.Dispatcher;

            if (dispatchObject != null)
                return dispatchObject.BeginInvoke(action);

            action();
            return null;
        }
    }
}
