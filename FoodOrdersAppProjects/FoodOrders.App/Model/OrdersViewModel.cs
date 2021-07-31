using FoodOrders.Model.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace FoodOrders.Model
{
    public class OrdersViewModel : DataViewModelBase<Order, OrderViewModel>
    {
        protected override void OnSelectedItemChanged(Order oldItem)
        {
            if (SelectedItem != null)
                UpdateDetails(SelectedItem);

            base.OnSelectedItemChanged(SelectedItem);
        }

        protected override void UpdateDetails(Order item)
        {
            using FoodOrdersContext context = FoodOrdersContext.GetFoodOrdersContext();
            context.UpdateDetails(item);
        }

        protected override bool CanExecuteFetchDataCommand()
        {
            return true;
        }

        protected override void ExecuteFetchDataCommand()
        {
            Debug.WriteLine($"{nameof(ExecuteFetchDataCommand)}", nameof(MainViewModel));

            using FoodOrdersContext context = FoodOrdersContext.GetFoodOrdersContext();
            context.Orders.Include(nameof(Order.Employee)).Load();
            UpdateItems(context.Orders);
        }

        protected override bool CanExecuteNewDataCommand()
        {
            return true;
        }

        protected override void ExecuteNewDataCommand()
        {
            using FoodOrdersContext context = FoodOrdersContext.GetFoodOrdersContext();
            throw new NotImplementedException();
        }
    }
}
