namespace FoodOrders.Model
{
    public partial class MainContext : ContextBase
    {
        public OrdersContext OrdersContext { get; }

        public MainContext()
        {
            OrdersContext = new OrdersContext();
            GenerateDataCommand = new GenerateDataCommandImpl(this);
        }
    }
}
