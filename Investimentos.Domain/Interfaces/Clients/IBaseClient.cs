using Investimentos.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investimentos.Domain.Interfaces.Clients
{
    public interface IBaseClient
    {
        Task<IEnumerable<InvestimentoModel>> GetInvestimentos();
    }
}