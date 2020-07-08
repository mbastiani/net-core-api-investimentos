using Investimentos.Domain.Enums;
using Investimentos.Domain.Interfaces.Clients;
using Investimentos.Domain.Models;
using Investimentos.Infra.Clients.Fundos.Models;
using Investimentos.Infra.Exceptions;
using Investimentos.Infra.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Investimentos.Infra.Clients.Fundos
{
    public class FundosClient : IFundosClient
    {
        private readonly ApiJsonSerializer _apiJsonSerializer;
        private readonly IHttpClientFactory _httpClientFactory;

        public FundosClient(
            IHttpClientFactory httpClientFactory,
            ApiJsonSerializer apiJsonSerializer)
        {
            _httpClientFactory = httpClientFactory;
            _apiJsonSerializer = apiJsonSerializer;
        }
        public async Task<IEnumerable<InvestimentoModel>> GetInvestimentos()
        {
            try
            {
                using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://www.mocky.io/v2/5e342ab33000008c00d96342");
                using var response = await _httpClientFactory.CreateClient().SendAsync(requestMessage);

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var data = _apiJsonSerializer.Deserialize<FundosGetInvestimentosResponse>(responseContent);
                if (data.Fundos is null)
                    return null;

                return data.Fundos.Select(x => new InvestimentoModel
                {
                    DataInvestimento = x.DataCompra,
                    Nome = x.Nome,
                    TipoInvestimento = TiposInvestimentoEnum.Fundos,
                    ValorInvestido = x.CapitalInvestido,
                    ValorTotal = x.ValorAtual,
                    Vencimento = x.DataResgate
                }).ToList();
            }
            catch(Exception ex)
            {
                throw new ApiException("Erro ao consultar o serviço de fundos", ex);
            }
        }
    }
}