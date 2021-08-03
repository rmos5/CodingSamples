using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;

namespace FoodOrders.Model.Data
{
    public partial class FoodOrdersDataContext : DbContext
    {
        protected static object syncRoot = new object();

        public static FoodOrdersDataContext GetFoodOrdersContext()
        {
            FoodOrdersDataContext result = new FoodOrdersDataContext();
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

        private int GetNextOrderNumber()
        {
            lock (syncRoot)
            {
                int result = Orders.CountAsync().Result == 0 ? 1 : Orders.MaxAsync(o => o.OrderNumber).Result + 1;
                return result;
            }
        }

        public bool HasChanges<TData>(TData item) 
            where TData : DataModelObjectBase, new()
        {
            EntityEntry<TData> entry = Attach(item);
            return entry.State == EntityState.Modified;
        }

        public void Create<TData>(TData item)
            where TData : DataModelObjectBase
        {
            item.Id = Guid.NewGuid();

            if (typeof(TData).Equals(typeof(Employee)))
            {
                Employees.Add(item as Employee);
            }
            else if (typeof(TData).Equals(typeof(Provider)))
            {
                Providers.Add(item as Provider);
            }
            else if (typeof(TData).Equals(typeof(Product)))
            {
                Products.Add(item as Product);
            }
            else if (typeof(TData).Equals(typeof(Order)))
            {
                Orders.Add(item as Order);
            }
            else
                throw new ArgumentException(typeof(TData).FullName);


            SaveChanges();
        }

        public new void Update<TData>(TData item)
            where TData : DataModelObjectBase
        {
            item.Id = Guid.NewGuid();

            if (typeof(TData).Equals(typeof(Employee)))
            {
                Employees.Update(item as Employee);
            }
            else if (typeof(TData).Equals(typeof(Provider)))
            {
                Providers.Update(item as Provider);
            }
            else if (typeof(TData).Equals(typeof(Product)))
            {
                Products.Update(item as Product);
            }
            else if (typeof(TData).Equals(typeof(Order)))
            {
                Orders.Add(item as Order);
            }
            else
                throw new ArgumentException(typeof(TData).FullName);


            SaveChanges();
        }

        public void Delete<TData>(TData item)
            where TData : DataModelObjectBase
        {
            if (typeof(TData).Equals(typeof(Employee)))
            {
                Employees.Remove(item as Employee);
            }
            else if (typeof(TData).Equals(typeof(Provider)))
            {
                Providers.Remove(item as Provider);
            }
            else if (typeof(TData).Equals(typeof(Product)))
            {
                Products.Remove(item as Product);
            }
            else if (typeof(TData).Equals(typeof(Order)))
            {
                Orders.Remove(item as Order);
            }
            else
                throw new ArgumentException(typeof(TData).FullName);

            SaveChanges();
        }

        public IEnumerable<TData> Load<TData>()
            where TData : DataModelObjectBase
        {
            IEnumerable<TData> result;

            if (typeof(TData).Equals(typeof(Employee)))
            {
                Employees.Load();
                result = (IEnumerable<TData>)Employees;
            }
            else if (typeof(TData).Equals(typeof(Provider)))
            {
                Providers.Load();
                result = (IEnumerable<TData>)Providers;
            }
            else if (typeof(TData).Equals(typeof(Product)))
            {
                Products.Load();
                result = (IEnumerable<TData>)Products;
            }
            else if (typeof(TData).Equals(typeof(Order)))
            {
                Orders.Load();
                result = (IEnumerable<TData>)Orders;
            }
            else
                throw new ArgumentException(typeof(TData).FullName);

            return result;
        }

        public void UpdateDetails<TData>(TData item)
            where TData : DataModelObjectBase
        {
            if (item is Employee)
                UpdateDetails(item as Employee);
            else if (item is Provider)
                UpdateDetails(item as Provider);
            else if (item is Product)
                UpdateDetails(item as Product);
            else if (item is OrderLine)
                UpdateDetails(item as OrderLine);
            else if (item is PaymentLine)
                UpdateDetails(item as PaymentLine);
            else if (item is Order)
                UpdateDetails(item as Order);
            else
                throw new ArgumentException(nameof(item));
        }

        public void UpdateDetails(Employee item)
        {
            EntityEntry<Employee> entry = Employees.Attach(item);
            entry.Reload();
        }

        public void UpdateDetails(Provider item)
        {
            EntityEntry<Provider> entry = Providers.Attach(item);
            entry.Reload();
            entry.Collection(nameof(Provider.Products)).Load();

            foreach (Product obj in item.Products)
                UpdateDetails(obj);
        }

        public void UpdateDetails(Product item)
        {
            EntityEntry<Product> entry = Products.Attach(item);
            entry.Reload();
            entry.Reference(nameof(Product.Provider)).Load();
        }

        public void UpdateDetails(OrderLine item)
        {
            EntityEntry<OrderLine> entry = Attach(item);
            entry.Reload();
            entry.Reference(nameof(OrderLine.Order)).Load();
            entry.Reference(nameof(OrderLine.Product)).Load();
        }

        public void UpdateDetails(PaymentLine item)
        {
            EntityEntry<PaymentLine> entry = Attach(item);
            entry.Reload();
            entry.Reference(nameof(PaymentLine.Order)).Load();
        }

        public void UpdateDetails(Order item)
        {
            EntityEntry<Order> entry = Orders.Attach(item);
            entry.Reload();
            entry.Reference(nameof(Order.Employee)).Load();

            entry.Collection(nameof(Order.OrderLines)).Load();

            foreach (OrderLine obj in item.OrderLines)
                UpdateDetails(obj);

            entry.Collection(nameof(Order.PaymentLines)).Load();

            foreach (PaymentLine obj in item.PaymentLines)
                UpdateDetails(obj);
        }
    }
}
