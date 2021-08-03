using FoodOrders.Model.Data;
using System.Collections.Generic;
using System.Diagnostics;

namespace FoodOrders.Model
{
    public abstract class FoodOrdersDataContextBase<TData, TContext> : DataContextBase<TData, TContext>
        where TData : DataModelObjectBase, new()
        where TContext : ContextBase<TData>, new()
    {
        protected FoodOrdersDataContext GetDataContext()
        {
            return FoodOrdersDataContext.GetFoodOrdersContext();
        }

        protected override bool CanExecuteFetchDataCommand(object parameter)
        {
            return true;
        }

        protected override void ExecuteFetchDataCommand(object parameter)
        {
            Debug.WriteLine($"{nameof(ExecuteFetchDataCommand)}:{parameter}", GetType().Name);

            using FoodOrdersDataContext context = GetDataContext();
            IEnumerable<TData> items = context.Load<TData>();
            UpdateItems(items);
        }

        protected override void UpdateDetails(TData item)
        {
            using FoodOrdersDataContext context = GetDataContext();
            context.UpdateDetails(item);
        }

        protected override bool HasChanges(TContext item)
        {
            using FoodOrdersDataContext context = GetDataContext();
            return context.HasChanges(item.Data);
        }

        protected override bool CanExecuteCreateDataCommand(object parameter)
        {
            return true;
        }

        protected override void ExecuteUpdateDataCommand(object parameter)
        {
            using FoodOrdersDataContext context = GetDataContext();
            context.Update(SelectedItem);
        }

        protected override void ExecuteDeleteDataCommand(object parameter)
        {
            using FoodOrdersDataContext context = GetDataContext();
            context.Delete(SelectedItem);
        }
    }
}
