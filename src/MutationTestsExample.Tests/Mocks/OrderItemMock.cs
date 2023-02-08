using Bogus;
using MutationTestsExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MutationTestsExample.Tests.Mocks;

public static class OrderItemMock
{
    public static OrderItem GetFaker()
    {
        return GetListFaker(1).FirstOrDefault();
    }

    public static IEnumerable<OrderItem> GetListFaker(int quantity)
    {
        var fake = new Faker<OrderItem>()
            .CustomInstantiator(x => new OrderItem(
                Guid.NewGuid(), 
                x.Commerce.Product(), 
                x.Random.Int(1, 10), 
                x.Random.Decimal(10, 50)));

        return fake.Generate(quantity);
    }
}
