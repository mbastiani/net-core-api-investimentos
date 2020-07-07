using System;
using System.Text.Json.Serialization;

namespace Investimentos.Infra.Clients.Fundos.Models
{
    public class FundosInvestimentos
    {
        public decimal CapitalInvestido { get; set; }

        [JsonPropertyName("ValorAtual")]
        public decimal ValorAtual { get; set; }
        public DateTime DataResgate { get; set; }
        public DateTime DataCompra { get; set; }
        public int Iof { get; set; }
        public string Nome { get; set; }
        public decimal TotalTaxas { get; set; }
        public int Quantity { get; set; }
    }
}