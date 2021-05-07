using FluentAssertions;
using MutationTestsExample.Models;
using MutationTestsExample.Tests.Mocks;
using System;
using Xunit;

namespace MutationTestsExample.Tests.Models
{
    public class OrderItemsTests
    {
        [Fact(DisplayName = "Deve calcular valor ordem item")]
        public void Deve_calcular_valor_ordem_item()
        {
            // Arrange
            var expectedResult = 30;
            var orderItem = new OrderItem(Guid.NewGuid(), "cimento", 2, 15);

            // Act
            var result = orderItem.CalcularValor();

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact(DisplayName = "Deve associar pedido")]
        public void Deve_Teste()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var orderItem = OrderItemMock.GetFaker();

            // Act
            orderItem.AssociarPedido(pedidoId);

            // Assert
            orderItem.PedidoId.Should().Be(pedidoId);
        }

        [Fact(DisplayName = "Deve adicionar unidades")]
        public void Deve_Teste1()
        {
            // Arrange
            var unidades = new Random().Next(10);
            var orderItem = OrderItemMock.GetFaker();
            var expectedResult = orderItem.Quantidade + unidades;

            // Act
            orderItem.AdicionarUnidades(unidades);

            // Assert
            orderItem.Quantidade.Should().Be(expectedResult);
        }

        [Fact(DisplayName = "Deve atualizar unidades")]
        public void Deve_Teste2()
        {
            // Arrange
            var unidades = new Random().Next(10);
            var orderItem = OrderItemMock.GetFaker();

            // Act
            orderItem.AtualizarUnidades(unidades);

            // Assert
            orderItem.Quantidade.Should().Be(unidades);
        }
    }
}
