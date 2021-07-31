using FoodOrders.Model.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace FoodOrders.Model
{
    public partial class EmployeesViewModel : DataViewModelBase<Employee, EmployeeViewModel>
    {
        protected override bool CanExecuteFetchDataCommand()
        {
            return true;
        }

        protected override void ExecuteFetchDataCommand()
        {
            Debug.WriteLine($"{nameof(ExecuteFetchDataCommand)}", nameof(MainViewModel));

            using FoodOrdersContext context = FoodOrdersContext.GetFoodOrdersContext();
            context.Employees.Load();
            UpdateItems(context.Employees);
        }

        protected override bool CanExecuteNewDataCommand()
        {
            return true;
        }

        protected override void ExecuteNewDataCommand()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateDetails(Employee item)
        {
            using FoodOrdersContext context = FoodOrdersContext.GetFoodOrdersContext();
            context.UpdateDetails(item);
        }
    }
}
