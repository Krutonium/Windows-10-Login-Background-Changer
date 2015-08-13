using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace MessageBoxLibrary
{
    /// <summary>
    /// This class allows delegating the commanding logic to methods passed as parameters,
    /// and enables a View to bind commands to objects that are not part of the element tree.
    /// </summary>
    public class DelegateCommand : ICommand
    {

        private readonly Action _mExecuteMethod;
        private readonly Func<bool> _mCanExecuteMethod;
        private bool _mIsAutomaticRequeryDisabled;
        private List<WeakReference> _mCanExecuteChangedHandlers;

        public DelegateCommand(Action executeMethod)
            : this(executeMethod, null, false) { }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : this(executeMethod, canExecuteMethod, false) { }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
        {
            if (executeMethod == null) throw new ArgumentNullException("executeMethod"); 

            _mExecuteMethod = executeMethod;
            _mCanExecuteMethod = canExecuteMethod;
            _mIsAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }

        public bool CanExecute()
        {
            if (_mCanExecuteMethod != null) return _mCanExecuteMethod(); 
            return true;
        }

        /// <summary>
        ///  Execution of the command
        /// </summary>
        public void Execute()
        {
            if (_mExecuteMethod != null) _mExecuteMethod(); 
        }

        /// <summary>
        ///  Property to enable or disable CommandManager's automatic requery on this command
        /// </summary>
        public bool IsAutomaticRequeryDisabled
        {
            get{ return _mIsAutomaticRequeryDisabled; }
            set
            {
                if (_mIsAutomaticRequeryDisabled != value)
                {
                    if (value) CommandManagerHelper.RemoveHandlersFromRequerySuggested(_mCanExecuteChangedHandlers); 
                    else CommandManagerHelper.AddHandlersToRequerySuggested(_mCanExecuteChangedHandlers); 
                    _mIsAutomaticRequeryDisabled = value;
                }
            }
        }
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        protected virtual void OnCanExecuteChanged()
        {
            CommandManagerHelper.CallWeakReferenceHandlers(_mCanExecuteChangedHandlers);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (!_mIsAutomaticRequeryDisabled) CommandManager.RequerySuggested += value; 
                CommandManagerHelper.AddWeakReferenceHandler(ref _mCanExecuteChangedHandlers, value, 2);
            }
            remove
            {
                if (!_mIsAutomaticRequeryDisabled) CommandManager.RequerySuggested -= value; 
                CommandManagerHelper.RemoveWeakReferenceHandler(_mCanExecuteChangedHandlers, value);
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

        private readonly Action<T> _mExecuteMethod;
        private readonly Func<T, bool> _mCanExecuteMethod;
        private bool _mIsAutomaticRequeryDisabled;
        private List<WeakReference> _mCanExecuteChangedHandlers;

        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, null, false) {}

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : this(executeMethod, canExecuteMethod, false) { }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
        {
            if (executeMethod == null) throw new ArgumentNullException("executeMethod"); 

            _mExecuteMethod = executeMethod;
            _mCanExecuteMethod = canExecuteMethod;
            _mIsAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }

        public bool CanExecute(T parameter)
        {
            if (_mCanExecuteMethod != null) return _mCanExecuteMethod(parameter); 
            return true;
        }

        public void Execute(T parameter)
        {
            if (_mExecuteMethod != null) _mExecuteMethod(parameter); 
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        protected virtual void OnCanExecuteChanged()
        {
            CommandManagerHelper.CallWeakReferenceHandlers(_mCanExecuteChangedHandlers);
        }

        public bool IsAutomaticRequeryDisabled
        {
            get{ return _mIsAutomaticRequeryDisabled; }
            set
            {
                if (_mIsAutomaticRequeryDisabled != value)
                {
                    if (value) CommandManagerHelper.RemoveHandlersFromRequerySuggested(_mCanExecuteChangedHandlers); 
                    else CommandManagerHelper.AddHandlersToRequerySuggested(_mCanExecuteChangedHandlers); 
                    _mIsAutomaticRequeryDisabled = value;
                }
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (!_mIsAutomaticRequeryDisabled) CommandManager.RequerySuggested += value; 
                CommandManagerHelper.AddWeakReferenceHandler(ref _mCanExecuteChangedHandlers, value, 2);
            }
            remove
            {
                if (!_mIsAutomaticRequeryDisabled) CommandManager.RequerySuggested -= value; 
                CommandManagerHelper.RemoveWeakReferenceHandler(_mCanExecuteChangedHandlers, value);
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            // if T is of value type and the parameter is not
            // set yet, then return false if CanExecute delegate
            // exists, else return true
            if (parameter == null && typeof(T).IsValueType) return (_mCanExecuteMethod == null); 
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
            if (x == null) return;
            // Take a snapshot of the handlers before we call out to them since the handlers
            // could cause the array to me modified while we are reading it.

            var callers = new EventHandler[x.Count];
            int count = 0;

            for (int i = x.Count - 1; i >= 0; i--)
            {
                var reference = x[i];
                var handler = reference.Target as EventHandler;
                if (handler == null)
                {
                    // Clean up old handlers that have been collected
                    x.RemoveAt(i);
                }
                else
                {
                    callers[count] = handler;
                    count++;
                }
            }

            // Call the handlers that we snapshotted
            for (int i = 0; i < count; i++)
            {
                var handler = callers[i];
                handler(null, EventArgs.Empty);
            }
        };

        internal static Action<List<WeakReference>> AddHandlersToRequerySuggested = x =>
        {
            if (x != null)
            {
                x.ForEach(y =>
                {
                    var handler = y.Target as EventHandler;
                    if (handler != null) CommandManager.RequerySuggested += handler;
                });
            }
        };

        internal static Action<List<WeakReference>> RemoveHandlersFromRequerySuggested = x =>
        {
            if (x != null)
            {
                x.ForEach(y =>
                {
                    var handler = y.Target as EventHandler;
                    if (handler != null) CommandManager.RequerySuggested -= handler; 
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
            if (x == null) return;
            for (int i = x.Count - 1; i >= 0; i--)
            {
                var reference = x[i];
                var existingHandler = reference.Target as EventHandler;
                if ((existingHandler == null) || (existingHandler == y))
                {
                    // Clean up old handlers that have been collected
                    // in addition to the handler that is to be removed.
                    x.RemoveAt(i);
                }
            }
        };        
    }
}