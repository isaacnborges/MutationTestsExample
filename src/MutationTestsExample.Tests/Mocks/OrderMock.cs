using Bogus;
using MutationTestsExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MutationTestsExample.Tests.Mocks;

public static class OrderMock
{
    public static Order GetFaker(int itemsQuantity = 1)
    {
        return GetListFaker(1, itemsQuantity).FirstOrDefault();
    }

    public static IEnumerable<Order> GetListFaker(int quantity, int itemsQuantity)
    {
        var fake = new Faker<Order>()
            .CustomInstantiator(x => new Order(
                Guid.NewGuid(),
                OrderItemMock.GetListFaker(itemsQuantity).ToList()));

        return fake.Generate(quantity);
    }
}
