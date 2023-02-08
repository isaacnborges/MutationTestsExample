using FluentAssertions;
using MutationTestsExample.Enums;
using MutationTestsExample.Models;
using MutationTestsExample.Tests.Mocks;
using System;
using System.Linq;
using Xunit;

namespace MutationTestsExample.Tests.Models;

public class OrderTests
{
    [Fact(DisplayName = "Should create an order")]
    public void ShouldCreateOrder()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var itemsQuantity = 2;
        var items = OrderItemMock.GetListFaker(itemsQuantity);
        var expectedTotal = items.Sum(i => i.Quantity * i.UnitaryValue);

        // Act
        var order = new Order(customerId, items.ToList());

        // Assert
        order.Status.Should().Be(OrderStatus.Created);
        order.Items.Should().HaveCount(itemsQuantity);
        order.Total.Should().Be(expectedTotal);
    }

    [Fact(DisplayName = "Should allow add exist item")]
    public void ShouldAllowAddExistItem()
    {
        // Arrange
        var order = OrderMock.GetFaker(2);
        var orderItem = OrderItemMock.GetFaker();
        orderItem.ProductId = order.Items.FirstOrDefault().ProductId;

        // Act
        var result = order.OrderItemExists(orderItem);

        // Assert
        result.Should().BeTrue();
    }

    [Fact(DisplayName = "Should validate if item exists")]
    public void ShouldValidateIfItemExists()
    {
        // Arrange
        var order = OrderMock.GetFaker(2);
        var orderItem = OrderItemMock.GetFaker();

        // Act
        var result = order.OrderItemExists(orderItem);

        // Assert
        result.Should().BeFalse();
    }

    [Fact(DisplayName = "Should calculate order total value")]
    public void ShouldCalculateOrderTotalValue()
    {
        // Arrange
        var order = OrderMock.GetFaker(2);
        var expectedTotal = order.Items.Sum(i => i.Quantity * i.UnitaryValue);

        // Act
        order.CalculateTotalOrder();

        // Assert
        order.Total.Should().Be(expectedTotal);
    }

    [Fact(DisplayName = "Should apply a value voucher")]
    public void ShouldApplyValueVoucher()
    {
        // Arrange
        var valorDesconto = 10;
        var order = OrderMock.GetFaker(2);
        var expectedTotal = order.Items.Sum(i => i.Quantity * i.UnitaryValue) - valorDesconto;
        var voucher = VoucherMock.GetFaker(DicountType.Value);
        voucher.DicountAmount = valorDesconto;

        // Act
        order.ApplyVoucher(voucher);

        // Assert
        order.Voucher.Should().NotBeNull();
        order.Voucher.DicountAmount.Value.Should().Be(valorDesconto);
        order.UsedVoucher.Should().BeTrue();
        order.Discount.Should().Be(valorDesconto);
        order.Total.Should().Be(expectedTotal);
    }

    [Fact(DisplayName = "Should apply a percent voucher")]
    public void ShouldAppplyPercentVoucher()
    {
        // Arrange
        var percent = 10;
        var order = OrderMock.GetFaker(2);
        var expectedDesconto = order.Total * percent / 100;
        var expectedTotal = order.Items.Sum(i => i.Quantity * i.UnitaryValue) - expectedDesconto;
        var voucher = VoucherMock.GetFaker(DicountType.Percent);
        voucher.Percent = percent;

        // Act
        order.ApplyVoucher(voucher);

        // Assert
        order.Voucher.Should().NotBeNull();
        order.Voucher.Percent.Value.Should().Be(percent);
        order.UsedVoucher.Should().BeTrue();
        order.Discount.Should().Be(expectedDesconto);
        order.Total.Should().Be(expectedTotal);
    }

    [Fact(DisplayName = "Should return an order with zero value applying a higher discount that sum of items")]
    public void ShouldReturnOrderWithZeroValueApplyingHigherDiscountThatSumOfItems()
    {
        // Arrange
        var order = OrderMock.GetFaker(2);
        var totalItems = order.Items.Sum(i => i.Quantity * i.UnitaryValue);
        var dicountAmount = totalItems + 10;
        var voucher = VoucherMock.GetFaker(DicountType.Value);
        voucher.DicountAmount = dicountAmount;

        // Act
        Action action = () => order.ApplyVoucher(voucher);

        // Assert
        action.Should().Throw<Exception>().WithMessage("Order with invalid value");
    }
    //--------------------------------------------------------------------------------------------------------------------------------------------

    //--------------------------------------------------------------------------------------------------------------------------------------------
    [Fact(DisplayName = "Should add item to the order")]
    public void ShouldAddItemToTheOrder()
    {
        // Arrange
        var expectedItems = 4;
        var itemsQuantity = 3;
        var order = OrderMock.GetFaker(itemsQuantity);
        var item = OrderItemMock.GetFaker();
        var expectedTotal = item.CalculateValue() + order.Total;

        // Act
        order.AddItem(item);

        // Assert
        order.Items.Should().HaveCount(expectedItems);
        order.Total.Should().Be(expectedTotal);
        item.OrderId.Should().Be(order.Id);
    }

    [Fact(DisplayName = "Should add exist item to the order")]
    public void ShouldAddExistItemToTheOrder()
    {
        // Arrange
        var itemsQuantity = 3;
        var order = OrderMock.GetFaker(itemsQuantity);
        var newItem = OrderItemMock.GetFaker();
        var itemExistente = order.Items.LastOrDefault();
        newItem.ProductId = itemExistente.ProductId;

        var itemExistenteTotal = (itemExistente.Quantity + newItem.Quantity) * itemExistente.UnitaryValue;
        var expectedTotal = order.Items.Where(x => !x.ProductId.Equals(newItem.ProductId)).Sum(i => i.CalculateValue()) + itemExistenteTotal;

        // Act
        order.AddItem(newItem);

        // Assert
        order.Items.Should().HaveCount(itemsQuantity);
        order.Total.Should().Be(expectedTotal);
        newItem.OrderId.Should().Be(order.Id);
    }

    [Fact(DisplayName = "Should update item to the order")]
    public void ShouldUpdateItemToTheOrder()
    {
        // Arrange
        var itemsQuantity = 3;
        var order = OrderMock.GetFaker(itemsQuantity);
        var item = OrderItemMock.GetFaker();
        item.ProductId = order.Items.LastOrDefault().ProductId;

        var expectedTotal = order.Items.Where(x => !x.ProductId.Equals(item.ProductId)).Sum(i => i.CalculateValue()) + item.CalculateValue();

        // Act
        order.UpdateItem(item);

        // Assert
        item.OrderId.Should().Be(order.Id);
        order.Items.Should().HaveCount(itemsQuantity);
        order.Total.Should().Be(expectedTotal);
    }

    [Fact(DisplayName = "Should remove item to the order")]
    public void ShouldRemoveItemToTheOrder()
    {
        // Arrange
        var expectedItems = 2;
        var itemsQuantity = 3;
        var order = OrderMock.GetFaker(itemsQuantity);

        var item = OrderItemMock.GetFaker();
        item.ProductId = order.Items.LastOrDefault().ProductId;

        var expectedTotal = order.Items.Where(x => !x.ProductId.Equals(item.ProductId)).Sum(i => i.CalculateValue());

        // Act
        order.RemoveItem(item);

        // Assert
        order.Items.Should().HaveCount(expectedItems);
        order.Total.Should().Be(expectedTotal);
    }

    [Fact(DisplayName = "Should approve order")]
    public void ShouldStartOrder()
    {
        // Arrange
        var order = OrderMock.GetFaker();

        // Act
        order.ApproveOrder();

        // Assert
        order.Status.Should().Be(OrderStatus.Approved);
    }

    [Fact(DisplayName = "Should finalize order")]
    public void ShouldFinalizeOrder()
    {
        // Arrange
        var order = OrderMock.GetFaker();

        // Act
        order.FinalizeOrder();

        // Assert
        order.Status.Should().Be(OrderStatus.Finalized);
    }

    [Fact(DisplayName = "Should cancel order")]
    public void ShouldCancelOrder()
    {
        // Arrange
        var order = OrderMock.GetFaker();

        // Act
        order.CancelOrder();

        // Assert
        order.Status.Should().Be(OrderStatus.Canceled);
    }
}
