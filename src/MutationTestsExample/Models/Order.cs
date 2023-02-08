using MutationTestsExample.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MutationTestsExample.Models;

public class Order
{
    public Guid Id { get; private set; }
    public OrderStatus Status { get; private set; }
    public Guid CustomerId { get; private set; }
    public decimal Discount { get; private set; }
    public decimal Total { get; private set; }
    public bool UsedVoucher { get; private set; }
    public virtual Voucher Voucher { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items;
    private readonly List<OrderItem> _items;

    public Order(Guid clienteId, List<OrderItem> items)
    {
        Id = Guid.NewGuid();
        CustomerId = clienteId;
        _items = items;

        CreateOrder();
        CalculateTotalOrder();
    }

    public void CalculateTotalOrder()
    {
        Total = Items.Sum(p => p.CalculateValue());
        CalculateTotalOrderWithDiscount();
    }

    public void ApplyVoucher(Voucher voucher)
    {
        if (voucher.Validate())
        {
            Voucher = voucher;
            UsedVoucher = true;
            CalculateTotalOrder();
        }
    }

    public void CalculateTotalOrderWithDiscount()
    {
        if (!UsedVoucher)
            return;

        decimal discount = 0;
        var total = Total;

        if (Voucher.DicountType == DicountType.Percent)
        {
            if (Voucher.Percent.HasValue)
            {
                discount = total * Voucher.Percent.Value / 100;
                total -= discount;
            }
        }
        else
        {
            if (Voucher.DicountAmount.HasValue)
            {
                discount = Voucher.DicountAmount.Value;
                total -= discount;
            }
        }

        if (total <= 0)
            throw new Exception("Order with invalid value");

        Total = total;
        Discount = discount;
    }

    public bool OrderItemExists(OrderItem item) => _items.Any(p => p.ProductId == item.ProductId);

    public void AddItem(OrderItem item)
    {
        item.SetOrder(Id);

        if (OrderItemExists(item))
        {
            var itemExists = _items.First(p => p.ProductId == item.ProductId);

            itemExists.AddUnits(item.Quantity);
            item = itemExists;

            _items.Remove(itemExists);
        }

        _items.Add(item);

        CalculateTotalOrder();
    }

    public void UpdateItem(OrderItem item)
    {
        item.SetOrder(Id);

        var existItem = Items.FirstOrDefault(p => p.ProductId == item.ProductId);

        _items.Remove(existItem);
        _items.Add(item);

        CalculateTotalOrder();
    }

    public void RemoveItem(OrderItem item)
    {
        var existItem = _items.FirstOrDefault(p => p.ProductId == item.ProductId);

        _items.Remove(existItem);

        CalculateTotalOrder();
    }

    public void CreateOrder()
    {
        Status = OrderStatus.Created;
    }

    public void ApproveOrder()
    {
        Status = OrderStatus.Approved;
    }

    public void FinalizeOrder()
    {
        Status = OrderStatus.Finalized;
    }

    public void CancelOrder()
    {
        Status = OrderStatus.Canceled;
    }
}
