using FluentAssertions;
using MutationTestsExample.Enums;
using MutationTestsExample.Models;
using MutationTestsExample.Tests.Mocks;
using System;
using System.Linq;
using Xunit;

namespace MutationTestsExample.Tests.Models
{
    public class OrderTests
    {
        [Fact(DisplayName = "Deve criar pedido")]
        public void Deve_criar_pedido()
        {
            // Arrange
            var clienteId = Guid.NewGuid();
            var itemsQuantity = 2;
            var items = OrderItemMock.GetListFaker(itemsQuantity);
            var expectedTotal = items.Sum(i => i.Quantidade * i.ValorUnitario);

            // Act
            var order = new Order(clienteId, items.ToList());

            // Assert
            order.Status.Should().Be(PedidoStatus.Rascunho);
            order.Items.Should().HaveCount(itemsQuantity);
            order.ValorTotal.Should().Be(expectedTotal);
        }

        [Fact(DisplayName = "Deve permitir adicionar item existente")]
        public void Deve_permitir_adicionar_item_existente()
        {
            // Arrange
            var order = OrderMock.GetFaker(2);
            var orderItem = OrderItemMock.GetFaker();
            orderItem.ProdutoId = order.Items.FirstOrDefault().ProdutoId;

            // Act
            var result = order.PedidoItemExistente(orderItem);

            // Assert
            result.Should().BeTrue();
        }

        [Fact(DisplayName = "Deve validar se item existente")]
        public void Deve_validar_se_item_existente()
        {
            // Arrange
            var order = OrderMock.GetFaker(2);
            var orderItem = OrderItemMock.GetFaker();

            // Act
            var result = order.PedidoItemExistente(orderItem);

            // Assert
            result.Should().BeFalse();
        }

        [Fact(DisplayName = "Deve calcular valor total pedido")]
        public void Deve_calcular_valor_total_pedido()
        {
            // Arrange
            var order = OrderMock.GetFaker(2);
            var expectedTotal = order.Items.Sum(i => i.Quantidade * i.ValorUnitario);

            // Act
            order.CalcularValorPedido();

            // Assert
            order.ValorTotal.Should().Be(expectedTotal);
        }

        [Fact(DisplayName = "Deve retornar pedido com valor zerado aplicando desconto maior do que a soma dos itens")]
        public void Deve_retornar_pedido_com_valor_zerado_aplicando_desconto_maior_do_que_a_soma_dos_itens()
        {
            // Arrange            
            var order = OrderMock.GetFaker(2);
            var totalItems = order.Items.Sum(i => i.Quantidade * i.ValorUnitario);
            var valorDesconto = totalItems + 10;
            var voucher = VoucherMock.GetFaker(TipoDescontoVoucher.Valor);
            voucher.ValorDesconto = valorDesconto;

            // Act
            Action action = () => order.AplicarVoucher(voucher);

            // Assert            
            action.Should().Throw<Exception>().WithMessage("Pedido com valor inválido");
        }

        [Fact(DisplayName = "Deve aplicar voucher valor")]
        public void Deve_aplicar_voucher_valor()
        {
            // Arrange
            var valorDesconto = 10;
            var order = OrderMock.GetFaker(2);
            var expectedTotal = order.Items.Sum(i => i.Quantidade * i.ValorUnitario) - valorDesconto;
            var voucher = VoucherMock.GetFaker(TipoDescontoVoucher.Valor);
            voucher.ValorDesconto = valorDesconto;

            // Act
            order.AplicarVoucher(voucher);

            // Assert
            order.Voucher.Should().NotBeNull();
            order.Voucher.ValorDesconto.Value.Should().Be(valorDesconto);
            order.VoucherUtilizado.Should().BeTrue();
            order.Desconto.Should().Be(valorDesconto);
            order.ValorTotal.Should().Be(expectedTotal);
        }

        [Fact(DisplayName = "Deve aplicar voucher percentual")]
        public void Deve_aplicar_voucher_percentual()
        {
            // Arrange
            var percentualDesconto = 10;
            var order = OrderMock.GetFaker(2);
            var expectedDesconto = order.ValorTotal * percentualDesconto / 100;
            var expectedTotal = order.Items.Sum(i => i.Quantidade * i.ValorUnitario) - expectedDesconto;
            var voucher = VoucherMock.GetFaker(TipoDescontoVoucher.Porcentagem);
            voucher.Percentual = percentualDesconto;

            // Act
            order.AplicarVoucher(voucher);

            // Assert
            order.Voucher.Should().NotBeNull();
            order.Voucher.Percentual.Value.Should().Be(percentualDesconto);
            order.VoucherUtilizado.Should().BeTrue();
            order.Desconto.Should().Be(expectedDesconto);
            order.ValorTotal.Should().Be(expectedTotal);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------
        //[Fact(DisplayName = "Deve retornar pedido com valor zerado aplicando desconto igual a soma dos itens")]
        //public void Deve_validar_pedido_zerado2()
        //{
        //    // Arrange            
        //    var order = OrderMock.GetFaker(2);
        //    var totalItems = order.Items.Sum(i => i.Quantidade * i.ValorUnitario);
        //    var valorDesconto = totalItems;
        //    var voucher = VoucherMock.GetFaker(TipoDescontoVoucher.Valor);
        //    voucher.ValorDesconto = valorDesconto;

        //    // Act
        //    Action action = () => order.AplicarVoucher(voucher);

        //    // Assert            
        //    action.Should().Throw<Exception>().WithMessage("Pedido com valor inválido");
        //}

        //[Fact(DisplayName = "Deve gerar exception ao atualizar item inexistente")]
        //public void Deve_gerar_exception_ao_atualizar_item_inexistente()
        //{
        //    // Arrange
        //    var itemsQuantity = 3;
        //    var order = OrderMock.GetFaker(itemsQuantity);
        //    var item = OrderItemMock.GetFaker();

        //    // Act
        //    Action action = () => order.AtualizarItem(item);

        //    // Assert            
        //    action.Should().Throw<NullReferenceException>().WithMessage("O item não pertence ao pedido");
        //}

        //[Fact(DisplayName = "Deve gerar exception ao remover item inexistente")]
        //public void Deve_gerar_exception_ao_remover_item_inexistente()
        //{
        //    // Arrange
        //    var itemsQuantity = 3;
        //    var order = OrderMock.GetFaker(itemsQuantity);
        //    var item = OrderItemMock.GetFaker();

        //    // Act
        //    Action action = () => order.RemoverItem(item);

        //    // Assert            
        //    action.Should().Throw<NullReferenceException>().WithMessage("O item não pertence ao pedido");
        //}
        //--------------------------------------------------------------------------------------------------------------------------------------------


        [Fact(DisplayName = "Deve adicionar item ao pedido")]
        public void Deve_adicionar_item_ao_pedido()
        {
            // Arrange
            var expectedItems = 4;
            var itemsQuantity = 3;
            var order = OrderMock.GetFaker(itemsQuantity);
            var item = OrderItemMock.GetFaker();
            var expectedTotal = order.Items.Sum(i => i.Quantidade * i.ValorUnitario) + item.Quantidade * item.ValorUnitario;

            // Act
            order.AdicionarItem(item);

            // Assert
            order.Items.Should().HaveCount(expectedItems);
            order.ValorTotal.Should().Be(expectedTotal);
            item.PedidoId.Should().Be(order.Id);
        }

        [Fact(DisplayName = "Deve adicionar item existente ao pedido")]
        public void Deve_adicionar_item_existente_ao_pedido()
        {
            // Arrange
            var itemsQuantity = 3;
            var order = OrderMock.GetFaker(itemsQuantity);
            var newItem = OrderItemMock.GetFaker();
            var itemExistente = order.Items.LastOrDefault();
            newItem.ProdutoId = itemExistente.ProdutoId;

            var itemExistenteTotal = (itemExistente.Quantidade + newItem.Quantidade) * itemExistente.ValorUnitario;
            var expectedTotal = order.Items.Where(x => !x.ProdutoId.Equals(newItem.ProdutoId)).Sum(i => i.Quantidade * i.ValorUnitario) + itemExistenteTotal;

            // Act
            order.AdicionarItem(newItem);

            // Assert
            order.Items.Should().HaveCount(itemsQuantity);
            order.ValorTotal.Should().Be(expectedTotal);
            newItem.PedidoId.Should().Be(order.Id);
        }

        [Fact(DisplayName = "Deve atualizar item ao pedido")]
        public void Deve_atualizar_item_ao_pedido()
        {
            // Arrange
            var itemsQuantity = 3;
            var order = OrderMock.GetFaker(itemsQuantity);
            var item = OrderItemMock.GetFaker();
            item.ProdutoId = order.Items.LastOrDefault().ProdutoId;

            var expectedTotal = order.Items.Where(x => !x.ProdutoId.Equals(item.ProdutoId)).Sum(i => i.Quantidade * i.ValorUnitario) + item.Quantidade * item.ValorUnitario;

            // Act
            order.AtualizarItem(item);

            // Assert
            order.Items.Should().HaveCount(itemsQuantity);
            order.ValorTotal.Should().Be(expectedTotal);
        }

        [Fact(DisplayName = "Deve remover item ao pedido")]
        public void Deve_remover_item_ao_pedido()
        {
            // Arrange
            var expectedItems = 2;
            var itemsQuantity = 3;
            var order = OrderMock.GetFaker(itemsQuantity);

            var item = OrderItemMock.GetFaker();
            item.ProdutoId = order.Items.LastOrDefault().ProdutoId;

            var expectedTotal = order.Items.Where(x => !x.ProdutoId.Equals(item.ProdutoId)).Sum(i => i.Quantidade * i.ValorUnitario);

            // Act
            order.RemoverItem(item);

            // Assert
            order.Items.Should().HaveCount(expectedItems);
            order.ValorTotal.Should().Be(expectedTotal);
        }

        [Fact(DisplayName = "Deve_iniciar_pedido")]
        public void Deve_iniciar_pedido()
        {
            // Arrange
            var order = OrderMock.GetFaker();

            // Act
            order.IniciarPedido();

            // Assert
            order.Status.Should().Be(PedidoStatus.Iniciado);
        }

        [Fact(DisplayName = "Deve finalizar pedido")]
        public void Deve_finalizar_pedido()
        {
            // Arrange
            var order = OrderMock.GetFaker();

            // Act
            order.FinalizarPedido();

            // Assert
            order.Status.Should().Be(PedidoStatus.Pago);
        }

        [Fact(DisplayName = "Deve cancelar pedido")]
        public void Deve_cancelar_pedido()
        {
            // Arrange
            var order = OrderMock.GetFaker();

            // Act
            order.CancelarPedido();

            // Assert
            order.Status.Should().Be(PedidoStatus.Cancelado);
        }
    }
}
