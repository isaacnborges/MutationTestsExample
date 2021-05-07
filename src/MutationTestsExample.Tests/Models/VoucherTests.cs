using FluentAssertions;
using MutationTestsExample.Enums;
using MutationTestsExample.Models;
using System;
using Xunit;

namespace MutationTestsExample.Tests.Models
{
    public class VoucherTests
    {

        [Fact(DisplayName = "Deve criar um voucher valido")]
        public void Deve_criar_um_voucher_valido()
        {
            // Arrange
            var voucher = new Voucher(TipoDescontoVoucher.Porcentagem, DateTime.Today.AddDays(1), 3, true);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            result.Should().BeTrue();
        }

        [Fact(DisplayName = "Deve validar um voucher expirado com data atual")]
        public void Deve_validar_um_voucher_expirado_com_data_atual()
        {
            // Arrange
            var voucher = new Voucher(TipoDescontoVoucher.Porcentagem, DateTime.Today, 3, true);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            result.Should().BeFalse();
        }

        [Fact(DisplayName = "Deve validar um voucher expirado")]
        public void Deve_validar_um_voucher_expirado_com_data_anterior()
        {
            // Arrange
            var voucher = new Voucher(TipoDescontoVoucher.Porcentagem, DateTime.Today.AddDays(-1), 3, true);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            result.Should().BeFalse();
        }

        [Fact(DisplayName = "Deve validar um voucher inativo")]
        public void Deve_validar_um_voucher_inativo()
        {
            // Arrange
            var voucher = new Voucher(TipoDescontoVoucher.Porcentagem, DateTime.Today.AddDays(-1), 3, false);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            result.Should().BeFalse();
        }

        [Theory(DisplayName = "Deve validar um voucher com quantidade inválida")]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void Deve_validar_um_voucher_com_quantidade_invalida(int quantidade)
        {
            // Arrange
            var voucher = new Voucher(TipoDescontoVoucher.Porcentagem, DateTime.Today.AddDays(-1), quantidade, true);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            result.Should().BeFalse();
        }

        //[Fact(DisplayName = "Deve validar um voucher inativo")]
        //public void Deve_validar_um_voucher_inativo()
        //{
        //    // Arrange
        //    var voucher = new Voucher(TipoDescontoVoucher.Porcentagem, DateTime.Today.AddDays(1), 3, false);

        //    // Act
        //    var result = voucher.ValidarSeAplicavel();

        //    // Assert
        //    result.Should().BeFalse();
        //}

        //[Theory(DisplayName = "Deve validar um voucher com quantidade inválida")]
        //[InlineData(0)]
        //[InlineData(-1)]
        //[InlineData(-10)]
        //public void Deve_validar_um_voucher_com_quantidade_invalida(int quantidade)
        //{
        //    // Arrange
        //    var voucher = new Voucher(TipoDescontoVoucher.Porcentagem, DateTime.Today.AddDays(1), quantidade, true);

        //    // Act
        //    var result = voucher.ValidarSeAplicavel();

        //    // Assert
        //    result.Should().BeFalse();
        //}
    }
}
