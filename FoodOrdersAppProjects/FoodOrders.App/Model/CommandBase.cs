using System;
using System.Windows.Input;

namespace FoodOrders.Model
{
    public abstract class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);

        public void RaiseCanExecuteChanged()
        {
            EventHandler h = CanExecuteChanged;
            h?.Invoke(this, EventArgs.Empty);
        }

    }

    public abstract class CommandBase<T> : CommandBase
       where T : ContextBase
    {
        public T Context { get; }

        protected CommandBase(T context) => Context = context ?? throw new ArgumentNullException(nameof(context));
    }
}
