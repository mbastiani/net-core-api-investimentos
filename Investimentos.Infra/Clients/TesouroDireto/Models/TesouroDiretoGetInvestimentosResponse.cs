using System.Collections.Generic;

namespace Investimentos.Infra.Clients.TesouroDireto.Models
{
    public class TesouroDiretoGetInvestimentosResponse
    {
        public IEnumerable<TesouroDiretoInvestimentos> Tds { get; set; }
    }
}