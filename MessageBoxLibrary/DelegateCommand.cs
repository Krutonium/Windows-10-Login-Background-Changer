namespace MessageBoxLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// This class allows delegating the commanding logic to methods passed as parameters,
    /// and enables a View to bind commands to objects that are not part of the element tree.
    /// </summary>
    public class DelegateCommand : ICommand
    {

        private readonly Action m_ExecuteMethod = null;
        private readonly Func<bool> m_CanExecuteMethod = null;
        private bool m_IsAutomaticRequeryDisabled = false;
        private List<WeakReference> m_CanExecuteChangedHandlers;

        public DelegateCommand(Action executeMethod)
            : this(executeMethod, null, false) { }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : this(executeMethod, canExecuteMethod, false) { }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
        {
            if (executeMethod == null) throw new ArgumentNullException("executeMethod"); 

            m_ExecuteMethod = executeMethod;
            m_CanExecuteMethod = canExecuteMethod;
            m_IsAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }

        public bool CanExecute()
        {
            if (m_CanExecuteMethod != null) return m_CanExecuteMethod(); 
            return true;
        }

        /// <summary>
        ///  Execution of the command
        /// </summary>
        public void Execute()
        {
            if (m_ExecuteMethod != null) m_ExecuteMethod(); 
        }

        /// <summary>
        ///  Property to enable or disable CommandManager's automatic requery on this command
        /// </summary>
        public bool IsAutomaticRequeryDisabled
        {
            get{ return m_IsAutomaticRequeryDisabled; }
            set
            {
                if (m_IsAutomaticRequeryDisabled != value)
                {
                    if (value) CommandManagerHelper.RemoveHandlersFromRequerySuggested(m_CanExecuteChangedHandlers); 
                    else CommandManagerHelper.AddHandlersToRequerySuggested(m_CanExecuteChangedHandlers); 
                    m_IsAutomaticRequeryDisabled = value;
                }
            }
        }
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        protected virtual void OnCanExecuteChanged()
        {
            CommandManagerHelper.CallWeakReferenceHandlers(m_CanExecuteChangedHandlers);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (!m_IsAutomaticRequeryDisabled) CommandManager.RequerySuggested += value; 
                CommandManagerHelper.AddWeakReferenceHandler(ref m_CanExecuteChangedHandlers, value, 2);
            }
            remove
            {
                if (!m_IsAutomaticRequeryDisabled) CommandManager.RequerySuggested -= value; 
                CommandManagerHelper.RemoveWeakReferenceHandler(m_CanExecuteChangedHandlers, value);
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        void ICommand.Execute(object parameter)
        {
            Execute();
        }
    }

    /// <summary>
    /// This class allows delegating the commanding logic to methods passed as parameters,
    /// and enables a View to bind commands to objects that are not part of the element tree.
    /// </summary>
    /// <typeparam name="T">Type of the parameter passed to the delegates</typeparam>
    public class DelegateCommand<T> : ICommand
    {

        private readonly Action<T> m_ExecuteMethod = null;
        private readonly Func<T, bool> m_CanExecuteMethod = null;
        private bool m_IsAutomaticRequeryDisabled = false;
        private List<WeakReference> m_CanExecuteChangedHandlers;

        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, null, false) {}

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : this(executeMethod, canExecuteMethod, false) { }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
        {
            if (executeMethod == null) throw new ArgumentNullException("executeMethod"); 

            m_ExecuteMethod = executeMethod;
            m_CanExecuteMethod = canExecuteMethod;
            m_IsAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }

        public bool CanExecute(T parameter)
        {
            if (m_CanExecuteMethod != null) return m_CanExecuteMethod(parameter); 
            return true;
        }

        public void Execute(T parameter)
        {
            if (m_ExecuteMethod != null) m_ExecuteMethod(parameter); 
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        protected virtual void OnCanExecuteChanged()
        {
            CommandManagerHelper.CallWeakReferenceHandlers(m_CanExecuteChangedHandlers);
        }

        public bool IsAutomaticRequeryDisabled
        {
            get{ return m_IsAutomaticRequeryDisabled; }
            set
            {
                if (m_IsAutomaticRequeryDisabled != value)
                {
                    if (value) CommandManagerHelper.RemoveHandlersFromRequerySuggested(m_CanExecuteChangedHandlers); 
                    else CommandManagerHelper.AddHandlersToRequerySuggested(m_CanExecuteChangedHandlers); 
                    m_IsAutomaticRequeryDisabled = value;
                }
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (!m_IsAutomaticRequeryDisabled) CommandManager.RequerySuggested += value; 
                CommandManagerHelper.AddWeakReferenceHandler(ref m_CanExecuteChangedHandlers, value, 2);
            }
            remove
            {
                if (!m_IsAutomaticRequeryDisabled) CommandManager.RequerySuggested -= value; 
                CommandManagerHelper.RemoveWeakReferenceHandler(m_CanExecuteChangedHandlers, value);
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            // if T is of value type and the parameter is not
            // set yet, then return false if CanExecute delegate
            // exists, else return true
            if (parameter == null && typeof(T).IsValueType) return (m_CanExecuteMethod == null); 
            return CanExecute((T)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute((T)parameter);
        }
    }

    /// <summary>
    ///     This class contains methods for the CommandManager that help avoid memory leaks by
    ///     using weak references.
    /// </summary>
    internal class CommandManagerHelper
    {
        internal static Action<List<WeakReference>> CallWeakReferenceHandlers = x =>
        {
            if (x != null)
            {
                // Take a snapshot of the handlers before we call out to them since the handlers
                // could cause the array to me modified while we are reading it.

                var __Callers = new EventHandler[x.Count];
                int __Count = 0;

                for (int i = x.Count - 1; i >= 0; i--)
                {
                    var __Reference = x[i];
                    var __Handler = __Reference.Target as EventHandler;
                    if (__Handler == null)
                    {
                        // Clean up old handlers that have been collected
                        x.RemoveAt(i);
                    }
                    else
                    {
                        __Callers[__Count] = __Handler;
                        __Count++;
                    }
                }

                // Call the handlers that we snapshotted
                for (int i = 0; i < __Count; i++)
                {
                    var __Handler = __Callers[i];
                    __Handler(null, EventArgs.Empty);
                }
            }
        };

        internal static Action<List<WeakReference>> AddHandlersToRequerySuggested = x =>
        {
            if (x != null)
            {
                x.ForEach(y =>
                {
                    var __Handler = y.Target as EventHandler;
                    if (__Handler != null) CommandManager.RequerySuggested += __Handler;
                });
            }
        };

        internal static Action<List<WeakReference>> RemoveHandlersFromRequerySuggested = x =>
        {
            if (x != null)
            {
                x.ForEach(y =>
                {
                    var __Handler = y.Target as EventHandler;
                    if (__Handler != null) CommandManager.RequerySuggested -= __Handler; 
                });
            }
        };
        
        internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler)
        {
            AddWeakReferenceHandler(ref handlers, handler, -1);
        }

        internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler, int defaultListSize)
        {
            if (handlers == null)
            {
                handlers = (defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>());
            }

            handlers.Add(new WeakReference(handler));
        }

        internal static Action<List<WeakReference>, EventHandler> RemoveWeakReferenceHandler = (x, y) =>
        {
            if (x != null)
            {
                for (int i = x.Count - 1; i >= 0; i--)
                {
                    var __Reference = x[i];
                    var __ExistingHandler = __Reference.Target as EventHandler;
                    if ((__ExistingHandler == null) || (__ExistingHandler == y))
                    {
                        // Clean up old handlers that have been collected
                        // in addition to the handler that is to be removed.
                        x.RemoveAt(i);
                    }
                }
            }
        };        
    }
}