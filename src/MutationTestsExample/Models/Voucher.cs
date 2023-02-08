using MutationTestsExample.Enums;
using System;

namespace MutationTestsExample.Models;

public class Voucher
{
    public Guid Id { get; private set; }
    public decimal? Percent { get; set; }
    public decimal? DicountAmount { get; set; }
    public int Quantity { get; set; }
    public DicountType DicountType { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool Active { get; set; }

    public Voucher(DicountType dicountType, DateTime expirationDate, int quantity, bool active)
    {
        Id = Guid.NewGuid();
        DicountType = dicountType;
        ExpirationDate = expirationDate;
        Quantity = quantity;
        Active = active;
    }

    public bool Validate()
    {
        if (ExpirationDate <= DateTime.Today)
            return false;

        if (!Active)
            return false;

        if (Quantity <= 0)
            return false;

        return true;
    }
}