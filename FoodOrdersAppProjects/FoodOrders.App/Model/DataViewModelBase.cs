using FoodOrders.Model.Data;

namespace FoodOrders.Model
{
    public abstract class DataViewModelBase<TData, TViewModel> : ItemsViewModel<TData, TViewModel>
        where TData : DataModelObjectBase
        where TViewModel : ViewModelBase<TData>, new()
    {
        #region Commands

        class FetchDataCommandImpl : CommandBase<DataViewModelBase<TData, TViewModel>>
        {
            public FetchDataCommandImpl(DataViewModelBase<TData, TViewModel> context)
                : base(context)
            {
            }

            public override bool CanExecute(object parameter) => Context.CanExecuteFetchDataCommand();

            public override void Execute(object parameter) => Context.ExecuteFetchDataCommand();
        }

        class NewDataCommandImpl : CommandBase<DataViewModelBase<TData, TViewModel>>
        {
            public NewDataCommandImpl(DataViewModelBase<TData, TViewModel> context)
                : base(context)
            {
            }

            public override bool CanExecute(object parameter) => Context.CanExecuteNewDataCommand();

            public override void Execute(object parameter) => Context.ExecuteNewDataCommand();
        }

        #endregion

        public CommandBase FetchDataCommand { get; }

        public CommandBase NewDataCommand { get; }

        public DataViewModelBase()
        {
            FetchDataCommand = new FetchDataCommandImpl(this);
            NewDataCommand = new NewDataCommandImpl(this);
        }

        void UpdateModelState()
        {
            FetchDataCommand.RaiseCanExecuteChanged();
            NewDataCommand.RaiseCanExecuteChanged();
        }

        public override void Cleanup()
        {
            base.Cleanup();
            UpdateModelState();
        }

        protected abstract bool CanExecuteFetchDataCommand();

        protected abstract void ExecuteFetchDataCommand();

        protected abstract bool CanExecuteNewDataCommand();

        protected abstract void ExecuteNewDataCommand();

        protected override void OnSelectedItemChanged(TData oldItem)
        {
            if (SelectedItem != null)
                UpdateDetails(SelectedItem);

            base.OnSelectedItemChanged(SelectedItem);
        }

        protected abstract void UpdateDetails(TData item);
    }
}
