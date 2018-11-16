namespace DSDTEditor.NET.Lib
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Windows.Input;

    /// <summary>
    /// A command whose sole purpose is to
    /// relay its functionality to other
    /// object by invoking delegates. The
    /// default return value for the CanExecute
    /// method is 'true'.
    /// </summary>
    internal class RelayCommand<T> : ICommand
    {
        #region Fields
        readonly Action<T> _execute = null;
        readonly Predicate<T> _canExecute = null;
        #endregion

        #region Constructors
        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this._execute = execute;
            this._canExecute = canExecute;
        }
        #endregion Constructors

        #region ICommand Members
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return this._canExecute == null ? true : _canExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this._canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (this._canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public void Execute(object parameter)
        {
            this._execute((T)parameter);
        }
        #endregion ICommand Members
    }

    /// <summary>
    /// A command whose sole purpose is to
    /// relay its functionality to other
    /// object by invoking delegates. The
    /// default return value for the CanExecute
    /// method is 'true'.
    /// </summary>
    internal class RelayCommand : ICommand
    {
        #region Fields
        readonly Action _execute;
        readonly Func<bool> _canExecute;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this._execute = execute;
            this._canExecute = canExecute;
        }
        #endregion

        #region ICommand Members
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return this._canExecute == null ? true : this._canExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this._canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (this._canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public void Execute(object parameter)
        {
            this._execute();
        }
        #endregion ICommand Members
    }
}
