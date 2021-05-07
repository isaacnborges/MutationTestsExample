using MutationTestsExample.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MutationTestsExample.Models
{
    public class Order
    {
        public Guid Id { get; private set; }
        public PedidoStatus Status { get; private set; }
        public Guid ClienteId { get; private set; }
        public bool VoucherUtilizado { get; private set; }
        public decimal Desconto { get; private set; }
        public decimal ValorTotal { get; private set; }
        public virtual Voucher Voucher { get; private set; }
        public IReadOnlyCollection<OrderItem> Items => _items;
        private readonly List<OrderItem> _items;

        public Order(Guid clienteId, List<OrderItem> items)
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;
            _items = items;

            TornarRascunho();
            CalcularValorPedido();
        }

        public void CalcularValorPedido()
        {
            ValorTotal = Items.Sum(p => p.CalcularValor());
            CalcularValorTotalDesconto();
        }

        public void AplicarVoucher(Voucher voucher)
        {
            if (voucher.ValidarSeAplicavel())
            {
                Voucher = voucher;
                VoucherUtilizado = true;
                CalcularValorPedido();
            }
        }

        public void CalcularValorTotalDesconto()
        {
            if (!VoucherUtilizado)
                return;

            decimal desconto = 0;
            var valor = ValorTotal;

            if (Voucher.TipoDescontoVoucher == TipoDescontoVoucher.Porcentagem)
            {
                if (Voucher.Percentual.HasValue)
                {
                    desconto = valor * Voucher.Percentual.Value / 100;
                    valor -= desconto;
                }
            }
            else
            {
                if (Voucher.ValorDesconto.HasValue)
                {
                    desconto = Voucher.ValorDesconto.Value;
                    valor -= desconto;
                }
            }

            if (valor <= 0)
                throw new Exception("Pedido com valor inválido");

            ValorTotal = valor;
            Desconto = desconto;
        }

        public bool PedidoItemExistente(OrderItem item) => _items.Any(p => p.ProdutoId == item.ProdutoId);

        public void AdicionarItem(OrderItem item)
        {
            item.AssociarPedido(Id);

            if (PedidoItemExistente(item))
            {
                var itemExistente = _items.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);
                itemExistente.AdicionarUnidades(item.Quantidade);
                item = itemExistente;

                _items.Remove(itemExistente);
            }

            item.CalcularValor();
            _items.Add(item);

            CalcularValorPedido();
        }

        public void AtualizarItem(OrderItem item)
        {
            item.AssociarPedido(Id);

            var itemExistente = Items.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);

            //VerificarItemExistente(itemExistente);

            _items.Remove(itemExistente);
            _items.Add(item);

            CalcularValorPedido();
        }

        public void RemoverItem(OrderItem item)
        {
            var itemExistente = _items.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);

            //VerificarItemExistente(itemExistente);

            _items.Remove(itemExistente);

            CalcularValorPedido();
        }

        //private void VerificarItemExistente(OrderItem itemExistente)
        //{
        //    if (itemExistente == null)
        //        throw new NullReferenceException("O item não pertence ao pedido");
        //}

        private void TornarRascunho()
        {
            Status = PedidoStatus.Rascunho;
        }

        public void IniciarPedido()
        {
            Status = PedidoStatus.Iniciado;
        }

        public void FinalizarPedido()
        {
            Status = PedidoStatus.Pago;
        }

        public void CancelarPedido()
        {
            Status = PedidoStatus.Cancelado;
        }
    }
}
