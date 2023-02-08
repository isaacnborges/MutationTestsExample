using FluentAssertions;
using MutationTestsExample.Models;
using MutationTestsExample.Tests.Mocks;
using System;
using Xunit;

namespace MutationTestsExample.Tests.Models;

public class OrderItemsTests
{
    [Fact(DisplayName = "Should calculate order item value")]
    public void ShouldCalculateOrderItemValue()
    {
        // Arrange
        var expectedResult = 30;
        var orderItem = new OrderItem(Guid.NewGuid(), "new product", 2, 15);

        // Act
        var result = orderItem.CalculateValue();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact(DisplayName = "Should set order")]
    public void Should_set_order()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderItem = OrderItemMock.GetFaker();

        // Act
        orderItem.SetOrder(orderId);

        // Assert
        orderItem.OrderId.Should().Be(orderId);
    }

    [Fact(DisplayName = "Should add units")]
    public void ShouldAddUnits()
    {
        // Arrange
        var units = new Random().Next(10);
        var orderItem = OrderItemMock.GetFaker();
        var expectedResult = orderItem.Quantity + units;

        // Act
        orderItem.AddUnits(units);

        // Assert
        orderItem.Quantity.Should().Be(expectedResult);
    }

    [Fact(DisplayName = "Should update units")]
    public void ShouldUpdateUnits()
    {
        // Arrange
        var units = new Random().Next(10);
        var orderItem = OrderItemMock.GetFaker();

        // Act
        orderItem.UpdateUnits(units);

        // Assert
        orderItem.Quantity.Should().Be(units);
    }
}
