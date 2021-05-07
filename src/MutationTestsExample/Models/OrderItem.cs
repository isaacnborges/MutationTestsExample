using System;

namespace MutationTestsExample.Models
{
    public class OrderItem
    {
        public Guid Id { get; private set; }
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }

        public OrderItem(Guid produtoId, string produtoNome, int quantidade, decimal valorUnitario)
        {
            Id = Guid.NewGuid();
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        public decimal CalcularValor() => Quantidade * ValorUnitario;

        public void AssociarPedido(Guid pedidoId) => PedidoId = pedidoId;

        public void AdicionarUnidades(int unidades) => Quantidade += unidades;

        public void AtualizarUnidades(int unidades) => Quantidade = unidades;
    }
}
