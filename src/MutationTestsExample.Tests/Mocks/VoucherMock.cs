using Bogus;
using MutationTestsExample.Enums;
using MutationTestsExample.Models;
using System;

namespace MutationTestsExample.Tests.Mocks;

public static class VoucherMock
{
    public static Voucher GetFaker(DicountType dicountType, bool active = true)
    {
        return new Faker<Voucher>()
            .CustomInstantiator(x => new Voucher(
                dicountType,
                DateTime.Today.AddDays(5),
                x.Random.Int(1, 10),
                active));
    }
}
