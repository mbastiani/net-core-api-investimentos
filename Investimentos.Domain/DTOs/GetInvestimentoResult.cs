using Investimentos.Domain.Models;
using System.Collections.Generic;

namespace Investimentos.Domain.DTOs
{
    public class GetInvestimentoResult
    {
        public decimal ValorTotal { get; set; }
        public IEnumerable<InvestimentoModel> Investimentos { get; set; }
    }
}