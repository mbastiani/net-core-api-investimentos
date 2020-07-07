using Investimentos.Domain.Enums;
using Investimentos.Domain.Interfaces.Clients;
using Investimentos.Domain.Models;
using Investimentos.Infra.Clients.TesouroDireto.Models;
using Investimentos.Infra.Util;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Investimentos.Infra.Clients.TesouroDireto
{
    public class TesouroDiretoClient : ITesouroDiretoClient
    {
        private readonly ApiJsonSerializer _apiJsonSerializer;
        private readonly IHttpClientFactory _httpClientFactory;

        public TesouroDiretoClient(
            IHttpClientFactory httpClientFactory,
            ApiJsonSerializer apiJsonSerializer)
        {
            _httpClientFactory = httpClientFactory;
            _apiJsonSerializer = apiJsonSerializer;
        }
        public async Task<IEnumerable<InvestimentoModel>> GetInvestimentos()
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://www.mocky.io/v2/5e3428203000006b00d9632a");
            using var response = await _httpClientFactory.CreateClient().SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            var data = _apiJsonSerializer.Deserialize<TesouroDiretoGetInvestimentosResponse>(responseContent);
            if (data.Tds is null)
                return null;

            return data.Tds.Select(x => new InvestimentoModel
            {
                DataInvestimento = x.DataDeCompra,
                Nome = x.Nome,
                TipoInvestimento = TiposInvestimentoEnum.TesouroDireto,
                ValorInvestido = x.ValorInvestido,
                ValorTotal = x.ValorTotal,
                Vencimento = x.Vencimento
            }).ToList();
        }
    }
}