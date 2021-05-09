using System;

namespace MutationTestsExample.Models
{
    public class OrderItem
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitaryValue { get; set; }

        public OrderItem(Guid productId, string productName, int quantity, decimal unitaryValue)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitaryValue = unitaryValue;
        }

        public decimal CalculateValue() => Quantity * UnitaryValue;

        public void SetOrder(Guid orderId) => OrderId = orderId;

        public void AddUnits(int units) => Quantity += units;

        public void UpdateUnits(int units) => Quantity = units;
    }
}
