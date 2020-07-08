using Investimentos.Domain.Enums;
using Investimentos.Domain.Interfaces.Clients;
using Investimentos.Domain.Models;
using Investimentos.Infra.Clients.RendaFixa.Models;
using Investimentos.Infra.Exceptions;
using Investimentos.Infra.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Investimentos.Infra.Clients.RendaFixa
{
    public class RendaFixaClient : IRendaFixaClient
    {
        private readonly ApiJsonSerializer _apiJsonSerializer;
        private readonly IHttpClientFactory _httpClientFactory;

        public RendaFixaClient(
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
                using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://www.mocky.io/v2/5e3429a33000008c00d96336");
                using var response = await _httpClientFactory.CreateClient().SendAsync(requestMessage);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var data = _apiJsonSerializer.Deserialize<RendaFixaGetInvestimentosResponse>(responseContent);
                if (data.Lcis is null)
                    return null;

                return data.Lcis.Select(x => new InvestimentoModel
                {
                    DataInvestimento = x.DataOperacao,
                    Nome = x.Nome,
                    TipoInvestimento = TiposInvestimentoEnum.RendaFixa,
                    ValorInvestido = x.CapitalInvestido,
                    ValorTotal = x.CapitalAtual,
                    Vencimento = x.Vencimento
                }).ToList();
            }
            catch(Exception ex)
            {
                throw new ApiException("Erro ao consultar o serviço de renda fixa", ex);
            }
        }
    }
}