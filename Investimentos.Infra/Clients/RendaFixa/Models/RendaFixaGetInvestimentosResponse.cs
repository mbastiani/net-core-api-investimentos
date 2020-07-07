using System.Collections.Generic;

namespace Investimentos.Infra.Clients.RendaFixa.Models
{
    public class RendaFixaGetInvestimentosResponse
    {
        public IEnumerable<RendaFixaInvestimentos> Lcis { get; set; }
    }
}