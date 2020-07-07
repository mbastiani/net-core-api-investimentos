using Investimentos.Domain.Enums;
using System;
using System.Text.Json.Serialization;

namespace Investimentos.Domain.Models
{
    public class InvestimentoModel
    {
        public string Nome { get; set; }
        public decimal ValorInvestido { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime Vencimento { get; set; }
        public decimal Ir { get; set; }
        public decimal ValorResgate { get; set; }

        [JsonIgnore]
        public DateTime DataInvestimento { get; set; }

        [JsonIgnore]
        public TiposInvestimentoEnum TipoInvestimento { get; set; }
    }
}