using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FoodOrders.Model.Data
{
    public partial class FoodOrdersContext : DbContext
    {
        protected static object syncRoot = new object();

        public static FoodOrdersContext GetFoodOrdersContext()
        {
            FoodOrdersContext result = new FoodOrdersContext();
            result.Database.EnsureCreated();

            return result;
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        
        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "Data Source=FoodOrdersData.db");
            //optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public void UpdateDetails(Employee item)
        {
            EntityEntry<Employee> entry = Employees.Attach(item);
            entry.Reload();
        }

        public void UpdateDetails(Order item)
        {
            EntityEntry<Order> entry = Orders.Attach(item);
            entry.Reference(nameof(Order.Employee)).Load();
            entry.Collection(nameof(Order.OrderLines)).Load();

            foreach (OrderLine obj in item.OrderLines)
                UpdateDetails(obj);

            entry.Collection(nameof(Order.PaymentLines)).Load();

            foreach (PaymentLine obj in item.PaymentLines)
                UpdateDetails(obj);
        }

        public void UpdateDetails(OrderLine item)
        {
            EntityEntry<OrderLine> entry = Attach(item);
            entry.Reference(nameof(OrderLine.Order)).Load();
            entry.Reference(nameof(OrderLine.Product)).Load();
        }

        public void UpdateDetails(PaymentLine item)
        {
            EntityEntry<PaymentLine> entry = Attach(item);
            entry.Reference(nameof(PaymentLine.Order)).Load();
        }
    }
}
