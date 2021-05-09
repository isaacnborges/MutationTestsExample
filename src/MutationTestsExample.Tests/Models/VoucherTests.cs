using FluentAssertions;
using MutationTestsExample.Enums;
using MutationTestsExample.Models;
using System;
using Xunit;

namespace MutationTestsExample.Tests.Models
{
    public class VoucherTests
    {

        [Fact(DisplayName = "Should create a valid voucher")]
        public void ShouldCreateValidVoucher()
        {
            // Arrange
            var voucher = new Voucher(DicountType.Percent, DateTime.Today.AddDays(1), 3, true);

            // Act
            var result = voucher.Validate();

            // Assert
            result.Should().BeTrue();
        }

        [Fact(DisplayName = "Should validate an expired voucher with actual date")]
        public void ShouldValidateExpiredVoucherWithActualDate()
        {
            // Arrange
            var voucher = new Voucher(DicountType.Percent, DateTime.Today, 3, true);

            // Act
            var result = voucher.Validate();

            // Assert
            result.Should().BeFalse();
        }

        [Fact(DisplayName = "Should validate an expired voucher with yesterday date")]
        public void ShouldValidateExpiredVoucherWithYesterdayDate()
        {
            // Arrange
            var voucher = new Voucher(DicountType.Percent, DateTime.Today.AddDays(-1), 3, true);

            // Act
            var result = voucher.Validate();

            // Assert
            result.Should().BeFalse();
        }

        [Fact(DisplayName = "Should validate an inactive voucher with yesterday date")]
        public void ShouldValidateInactiveVoucherWithYesterdayDate()
        {
            // Arrange
            var voucher = new Voucher(DicountType.Percent, DateTime.Today.AddDays(-1), 3, false);

            // Act
            var result = voucher.Validate();

            // Assert
            result.Should().BeFalse();
        }

        [Theory(DisplayName = "Should validate a voucher with invalid quantity with yesterday date")]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void ShouldValidateVoucherWithInvalidQuantityWithYesterdayDate(int quantity)
        {
            // Arrange
            var voucher = new Voucher(DicountType.Percent, DateTime.Today.AddDays(-1), quantity, true);

            // Act
            var result = voucher.Validate();

            // Assert
            result.Should().BeFalse();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------
        [Fact(DisplayName = "Should validate an inactive voucher")]
        public void ShouldValidateInactiveVoucher()
        {
            // Arrange
            var voucher = new Voucher(DicountType.Percent, DateTime.Today.AddDays(1), 3, false);

            // Act
            var result = voucher.Validate();

            // Assert
            result.Should().BeFalse();
        }

        [Theory(DisplayName = "Should validate a voucher with invalid quantity")]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void ShouldValidateVoucherWithInvalidQuantity(int quantity)
        {
            // Arrange
            var voucher = new Voucher(DicountType.Percent, DateTime.Today.AddDays(1), quantity, true);

            // Act
            var result = voucher.Validate();

            // Assert
            result.Should().BeFalse();
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------
    }
}
