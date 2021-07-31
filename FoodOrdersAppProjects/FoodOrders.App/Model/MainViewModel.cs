namespace FoodOrders.Model
{
    public partial class MainViewModel : ViewModelBase
    {
        public EmployeesViewModel EmployeesViewModel { get; }
        public OrdersViewModel OrdersViewModel { get; }

        public MainViewModel()
        {
            OrdersViewModel = new OrdersViewModel();
            EmployeesViewModel = new EmployeesViewModel();
            GenerateDataCommand = new GenerateDataCommandImpl(this);
        }
    }
}
