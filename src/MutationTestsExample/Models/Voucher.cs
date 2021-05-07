using MutationTestsExample.Enums;
using System;

namespace MutationTestsExample.Models
{
    public class Voucher
    {
        public Guid Id { get; private set; }
        public decimal? Percentual { get; set; }
        public decimal? ValorDesconto { get; set; }
        public int Quantidade { get; set; }
        public TipoDescontoVoucher TipoDescontoVoucher { get; set; }
        public DateTime DataValidade { get; set; }
        public bool Ativo { get; set; }

        public Voucher(TipoDescontoVoucher tipoDesconto, DateTime dataValidade, int quantidade, bool ativo)
        {
            Id = Guid.NewGuid();
            TipoDescontoVoucher = tipoDesconto;
            DataValidade = dataValidade;
            Quantidade = quantidade;
            Ativo = ativo;
        }

        public bool ValidarSeAplicavel()
        {
            if (DataValidade <= DateTime.Today)
                return false;

            if (!Ativo)
                return false;

            if (Quantidade <= 0)
                return false;

            return true;
        }
    }
}