using FoodOrders.Model.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodOrders.DataGenerator
{
    class Program
    {
        private static FoodOrdersDataContext GetDbContext()
        {
            return FoodOrdersDataContext.GetFoodOrdersContext();
        }

        static void Main(string[] args)
        {
            using FoodOrdersDataContext context = GetDbContext();
            RemoveAllData(context);
            CreateData(context);
        }

        private static void RemoveAllData(FoodOrdersDataContext context)
        {
            RemoveAllData(context, context.Orders.SelectMany(o => o.OrderLines).AsQueryable());
            RemoveAllData(context, context.Orders.SelectMany(o => o.PaymentLines).AsQueryable());
            RemoveAllData(context, context.Orders);
            RemoveAllData(context, context.Providers);
            RemoveAllData(context, context.Employees);
        }

        static void RemoveAllData<T>(FoodOrdersDataContext context, IQueryable<T> data)
            where T : class
        {
            data.Load();
            WriteData(data);
            context.RemoveRange(data);
            context.SaveChanges();
            data.Load();
        }

        static void WriteData<T>(IEnumerable<T> data)
            where T : class
        {
            Console.WriteLine($"***Start data: {typeof(T).Name}***");
            foreach (T obj in data)
                Console.WriteLine(obj.ToString());
            Console.WriteLine($"***End data: {typeof(T).Name}***");
        }

        static Random Random { get; } = new Random();

        static int NextRandom()
        {
            return Random.Next(5, 30);
        }

        private static void CreateData(FoodOrdersDataContext context)
        {
            CreateData(context, context.Employees, 20);
            CreateData(context, context.Providers, 50);
            CreateData(context, context.Orders, 100);
        }

        static IEnumerable<DateTime> GetRandomDates(DateTime seed, int count)
        {
            DateTime dt = seed;
            Random random = new Random();
            int d;
            int j = random.Next(1, 21);
            for (int i = 0; i < count; i++)
            {
                j--;
                if(j == 0)
                {
                    d = -1;
                    j = random.Next(1, 21);
                } else
                {
                    d = 0;
                }

                dt = dt.Date.AddDays(d);
                dt = CreateNextTime(dt);
                yield return dt;
            }

            DateTime CreateNextTime(DateTime seed)
            {
                int s = random.Next(seed.Second, 59);
                int m = random.Next(seed.Minute, 59);
                int h = random.Next(seed.Hour, 23);
                return new DateTime(seed.Year, seed.Month, seed.Day, h, m, s);
            }
        }

        static void CreateData(FoodOrdersDataContext context, DbSet<Employee> data, int count)
        {
            using IEnumerator<DateTime> dates = GetRandomDates(DateTime.Now, count).OrderBy(o => o).GetEnumerator();
            Employee employee;
            for (int i = 1; i <= count; i++)
            {
                dates.MoveNext();
                employee = CreateEmployee(dates.Current, $"First{i}", $"Last{i}", $"Nick{i}");
                data.Add(employee);
            }

            context.SaveChanges();
            data.Load();
        }

        static void CreateData(FoodOrdersDataContext context, DbSet<Provider> data, int count)
        {
            Provider provider;
            for (int i = 1; i <= count; i++)
            {
                provider = CreateProvider(i);
                data.Add(provider);
            }

            context.SaveChanges();
            data.Load();
        }

        static void CreateData(FoodOrdersDataContext context, DbSet<Order> data, int count)
        {
            using (IEnumerator<DateTime> dates = GetRandomDates(DateTime.Now, count).OrderBy(o => o).GetEnumerator())
            {
                List<Employee> employees = context.Employees.ToList();
                int index, number = 0; Order order; DateTime dt, dt2 = default;
                for (int i = 1; i <= count; i++)
                {
                    dates.MoveNext();
                    dt = dates.Current;

                    if (!dt.Date.Equals(dt2.Date))
                    {
                        number = 0;
                    }

                    number++;
                    dt2 = dt;
                    index = Random.Next(0, employees.Count - 1);
                    order = CreateOrder(context, dt, number, employees[index]);
                    data.Add(order);

                }
            }

            context.SaveChanges();
            data.Load();
        }

        private static Employee CreateEmployee(DateTime dateCreated, string firstName, string lastName, string nickname)
        {
            return new Employee
            {
                DateCreated = dateCreated.ToUniversalTime(),
                FirstName = firstName,
                LastName = lastName,
                Nickname = nickname
            };
        }

        private static Provider CreateProvider(int number)
        {
            Provider result = new Provider
            {
                Name = $"Provider{number}",
            };

            Product product;
            int count = NextRandom();
            for (int i = 1; i <= count; i++)
            {
                product = CreateProduct(i, result);
                result.Products.Add(product);
            }

            return result;
        }

        private static void UpdateProductPrices(Product result)
        {
            result.MaxPrice = Random.Next(0, 100);
            result.MaxPrice += Convert.ToDecimal(Random.NextDouble().ToString("N2"));
            int minPricePercent = Random.Next(0, 10);
            result.MinPrice = Convert.ToDecimal(((100 - minPricePercent) * result.MaxPrice / 100).ToString("N2"));
        }

        private static Product CreateProduct(int number, Provider provider)
        {
            Product result = new Product
            {
                Provider = provider,
                Name = $"Product{number}",
            };

            UpdateProductPrices(result);

            return result;
        }

        private static OrderLine CreateOrderLine(Order order, Product product, int lineNumber, decimal price, int quantity)
        {
            OrderLine result = new OrderLine
            {
                Order = order,
                Product = product,
                LineNumber = lineNumber,
                Price = price,
                Quantity = quantity
            };

            return result;
        }

        private static PaymentLine CreatePaymentLine(Order order, PaymentTypes paymentType, decimal amount)
        {
            PaymentLine result = new PaymentLine
            {
                Order = order,
                PaymentType = paymentType,
                Amount = amount
            };

            return result;
        }

        private static Order CreateOrder(FoodOrdersDataContext context, DateTime orderDate, int orderNumber, Employee employee)
        {
            Order result = new Order
            {
                OrderDate = orderDate.ToUniversalTime(),
                OrderNumber = orderNumber,
                Employee = employee,
            };

            List<Product> products = context.Products.ToList();
            int index, quantity, count = Random.Next(1, 10);
            Product product; OrderLine orderLine;
            for (int i = 1; i <= count; i++)
            {
                index = Random.Next(0, products.Count);
                if (index == 0)
                    continue;
                index--;
                product = products[index];
                quantity = Random.Next(1, 10);
                orderLine = CreateOrderLine(result, product, i, product.MaxPrice, quantity);
                result.OrderLines.Add(orderLine);
            }

            PaymentTypes[] paymentTypes = new[] { PaymentTypes.Card, PaymentTypes.Cash };
            int idx = Random.Next(1, 100) % 2 == 0 ? 0 : 1;
            PaymentTypes paymentType = paymentTypes[idx];
            decimal amount = result.OrderLines.Sum(o => o.Quantity * o.Price);
            result.ProductsAmountTotal = amount;

            result.PaymentLines.Add(CreatePaymentLine(result, paymentType, amount));
            result.PaymentsAmountTotal = result.PaymentLines.Sum(o => o.Amount);

            return result;
        }
    }
}
