using FoodOrders.Model.Data;
using System;
using System.Diagnostics;

namespace FoodOrders.Model
{
    public abstract class DataContextBase<TData, TContext> : ItemsContext<TData, TContext>
        where TData : DataModelObjectBase, new()
        where TContext : ContextBase<TData>, new()
    {
        #region Commands

        class FetchDataCommandImpl : CommandBase<DataContextBase<TData, TContext>>
        {
            public FetchDataCommandImpl(DataContextBase<TData, TContext> context)
                : base(context)
            {
            }

            public override bool CanExecute(object parameter) => Context.CanExecuteFetchDataCommand(parameter);

            public override void Execute(object parameter) => Context.ExecuteFetchDataCommand(parameter);
        }

        class CreateDataCommandImpl : CommandBase<DataContextBase<TData, TContext>>
        {
            public CreateDataCommandImpl(DataContextBase<TData, TContext> context)
                : base(context)
            {
            }

            public override bool CanExecute(object parameter) => Context.CanExecuteCreateDataCommand(parameter);

            public override void Execute(object parameter) => Context.ExecuteCreateDataCommand(parameter);
        }

        class UpdateDataCommandImpl : CommandBase<DataContextBase<TData, TContext>>
        {
            public UpdateDataCommandImpl(DataContextBase<TData, TContext> context)
                : base(context)
            {
            }

            public override bool CanExecute(object parameter) => Context.CanExecuteUpdateDataCommand(parameter);

            public override void Execute(object parameter) => Context.ExecuteUpdateDataCommand(parameter);
        }

        class DeleteDataCommandImpl : CommandBase<DataContextBase<TData, TContext>>
        {
            public DeleteDataCommandImpl(DataContextBase<TData, TContext> context)
                : base(context)
            {
            }

            public override bool CanExecute(object parameter) => Context.CanExecuteDeleteDataCommand(parameter);

            public override void Execute(object parameter) => Context.ExecuteDeleteDataCommand(parameter);
        }

        #endregion

        public CommandBase FetchDataCommand { get; }

        public CommandBase CreateDataCommand { get; }

        public CommandBase UpdateDataCommand { get; }

        public CommandBase DeleteDataCommand { get; }

        public DataContextBase()
        {
            FetchDataCommand = new FetchDataCommandImpl(this);
            CreateDataCommand = new CreateDataCommandImpl(this);
            UpdateDataCommand = new UpdateDataCommandImpl(this);
            DeleteDataCommand = new DeleteDataCommandImpl(this);
        }

        void UpdateModelState()
        {
            FetchDataCommand.RaiseCanExecuteChanged();
            CreateDataCommand.RaiseCanExecuteChanged();
        }

        public override void Cleanup()
        {
            base.Cleanup();
            UpdateModelState();
        }

        protected abstract bool CanExecuteFetchDataCommand(object parameter);

        protected abstract void ExecuteFetchDataCommand(object parameter);

        protected abstract bool CanExecuteCreateDataCommand(object parameter);

        protected virtual TData CreateNew(object parameter)
        {
            return new TData();
        }

        protected void ExecuteCreateDataCommand(object parameter)
        {
            Debug.WriteLine($"{nameof(ExecuteCreateDataCommand)}:{parameter}", GetType().Name);
            TData item = CreateNew(parameter);
            AddData(item);
            SelectedItem = item;
        }

        protected abstract void UpdateDetails(TData item);

        protected override void OnSelectedItemChanged(TData oldItem)
        {
            if (SelectedItem != null)
                UpdateDetails(SelectedItem);

            base.OnSelectedItemChanged(oldItem);
        }

        protected abstract bool HasChanges(TContext item);

        protected virtual bool CanExecuteUpdateDataCommand(object parameter)
        {
            return HasChanges(SelectedContext);
        }

        protected abstract void ExecuteUpdateDataCommand(object parameter);

        protected virtual bool CanExecuteDeleteDataCommand(object parameter)
        {
            return false;
        }

        protected abstract void ExecuteDeleteDataCommand(object parameter);
    }
}
