using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FoodOrders.Model.Data
{
    public abstract class DataModelObjectBase
    {
        [Key]
        public Guid Id { get; set; }
    }

    public class Employee : DataModelObjectBase
    {
        public DateTime DateCreated { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{LastName} {FirstName}";

        public string Nickname { get; set; }

        public override string ToString()
        {
            return $"{DateCreated:s};{FullName};{Nickname}";
        }
    }

    public class Provider : DataModelObjectBase
    {
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; private set; } = new ObservableCollection<Product>();

        public override string ToString()
        {
            return $"{Name}";
        }
    }

    public class Product : DataModelObjectBase
    {
        public string Name { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }

        public Guid ProviderId { get; set; }

        public virtual Provider Provider { get; set; }

        public override string ToString()
        {
            return $"{Name};{MinPrice};{MaxPrice}";
        }
    }

    public class Order : DataModelObjectBase
    {
        public string OrderId => $"{OrderNumber}/{OrderDate:dd-MM-yyyy}";

        public int OrderNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual ICollection<OrderLine> OrderLines { get; private set; } = new ObservableCollection<OrderLine>();

        public virtual ICollection<PaymentLine> PaymentLines { get; private set; } = new ObservableCollection<PaymentLine>();

        public decimal ProductsAmountTotal { get; set; }

        public decimal PaymentsAmountTotal { get; set; }

        public override string ToString()
        {
            return $"{OrderId};{ProductsAmountTotal:C2}{PaymentsAmountTotal:C2}";
        }
    }

    public class OrderLine : DataModelObjectBase
    {
        public int LineNumber { get; set; }

        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
        public virtual int Quantity { get; set; }

        public virtual decimal Price { get; set; }

        public decimal PriceTotal => Quantity * Price;

        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }

        public override string ToString()
        {
            return $"{Quantity};{Price:C2};{PriceTotal:C2}";
        }
    }

    public enum PaymentTypes
    {
        Unknown,
        Cash,
        Card
    }

    public class PaymentLine : DataModelObjectBase
    {
        public PaymentTypes PaymentType { get; set; }

        public decimal Amount { get; set; }

        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }

        public override string ToString()
        {
            return $"{PaymentType};{Amount:C2}";
        }
    }

}
