using Investimentos.Domain.DTOs;
using Investimentos.Domain.Enums;
using Investimentos.Domain.Interfaces.Clients;
using Investimentos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Investimentos.Service
{
    public class InvestimentoService
    {
        private readonly IFundosClient _fundosClient;
        private readonly IRendaFixaClient _rendaFixaClient;
        private readonly ITesouroDiretoClient _tesouroDiretoClient;

        public InvestimentoService(
            IFundosClient fundosClient,
            IRendaFixaClient rendaFixaClient,
            ITesouroDiretoClient tesouroDiretoClient)
        {
            _fundosClient = fundosClient;
            _rendaFixaClient = rendaFixaClient;
            _tesouroDiretoClient = tesouroDiretoClient;
        }

        public decimal CalcularIR(decimal valorTotal, decimal valorInvestido, TiposInvestimentoEnum tipoInvestimento)
        {
            var rentabilidade = valorTotal - valorInvestido;
            if (rentabilidade <= 0)
                return 0;

            return tipoInvestimento switch
            {
                TiposInvestimentoEnum.Fundos => rentabilidade * 0.15M,
                TiposInvestimentoEnum.RendaFixa => rentabilidade * 0.05M,
                TiposInvestimentoEnum.TesouroDireto => rentabilidade * 0.10M,
                _ => 0,
            };
        }

        public decimal CalcularValorResgate(decimal valorTotal, DateTime dataInvestimento, DateTime dataVencimento)
        {
            if ((dataVencimento - DateTime.Today).TotalDays <= 0)
            {
                return valorTotal;
            }
            else if ((dataVencimento - DateTime.Today).TotalDays <= 90)
            {
                return valorTotal - (valorTotal * 0.06M);
            }
            else
            {
                var tempoTotalCustodia = (dataVencimento - dataInvestimento).TotalDays;
                var tempoRestanteCustoria = (dataVencimento - DateTime.Today).TotalDays;

                if (tempoRestanteCustoria < (tempoTotalCustodia / 2))
                    return valorTotal - (valorTotal * 0.15M);
                else
                    return valorTotal - (valorTotal * 0.30M);
            }
        }

        public async Task<GetInvestimentoResult> GetInvestimentos()
        {
            try
            {
                var investimentos = new List<InvestimentoModel>();
                investimentos.AddRange(await _fundosClient.GetInvestimentos());
                investimentos.AddRange(await _rendaFixaClient.GetInvestimentos());
                investimentos.AddRange(await _tesouroDiretoClient.GetInvestimentos());

                investimentos.ForEach(x =>
                {
                    x.Ir = CalcularIR(x.ValorTotal, x.ValorInvestido, x.TipoInvestimento);
                    x.ValorResgate = CalcularValorResgate(x.ValorTotal, x.DataInvestimento, x.Vencimento);
                });

                return new GetInvestimentoResult
                {
                    Investimentos = investimentos,
                    ValorTotal = investimentos.Sum(x => x.ValorTotal)
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
