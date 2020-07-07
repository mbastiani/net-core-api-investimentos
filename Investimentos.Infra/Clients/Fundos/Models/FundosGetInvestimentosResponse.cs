using System.Collections.Generic;

namespace Investimentos.Infra.Clients.Fundos.Models
{
    public class FundosGetInvestimentosResponse
    {
        public IEnumerable<FundosInvestimentos> Fundos { get; set; }
    }
}