using Investimentos.Domain.Enums;
using Investimentos.Domain.Interfaces.Clients;
using Investimentos.Domain.Models;
using Investimentos.Service;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Investimentos.Tests
{
    public class InvestimentoServiceTests
    {
        private readonly InvestimentoService _investimentoService;
        private readonly IFundosClient _fundosClientMock;
        private readonly IRendaFixaClient _rendaFixaClientMock;
        private readonly ITesouroDiretoClient _tesouroDiretoClientMock;

        public InvestimentoServiceTests()
        {
            _fundosClientMock = Substitute.For<IFundosClient>();
            _fundosClientMock.GetInvestimentos().Returns(new List<InvestimentoModel>
            {
                new InvestimentoModel
                {
                    DataInvestimento = DateTime.Today,
                    TipoInvestimento = TiposInvestimentoEnum.Fundos,
                    ValorInvestido = 100,
                    ValorTotal = 150,
                    Vencimento = DateTime.Today.AddYears(1)
                }
            });

            _rendaFixaClientMock = Substitute.For<IRendaFixaClient>();
            _rendaFixaClientMock.GetInvestimentos().Returns(new List<InvestimentoModel>
            {
                new InvestimentoModel
                {
                    DataInvestimento = DateTime.Today,
                    TipoInvestimento = TiposInvestimentoEnum.RendaFixa,
                    ValorInvestido = 100,
                    ValorTotal = 150,
                    Vencimento = DateTime.Today.AddYears(1)
                }
            });

            _tesouroDiretoClientMock = Substitute.For<ITesouroDiretoClient>();
            _tesouroDiretoClientMock.GetInvestimentos().Returns(new List<InvestimentoModel>
            {
                new InvestimentoModel
                {
                    DataInvestimento = DateTime.Today,
                    TipoInvestimento = TiposInvestimentoEnum.TesouroDireto,
                    ValorInvestido = 100,
                    ValorTotal = 150,
                    Vencimento = DateTime.Today.AddYears(1)
                }
            });

            _investimentoService = new InvestimentoService(
                _fundosClientMock,
                _rendaFixaClientMock,
                _tesouroDiretoClientMock);
        }

        [Fact]
        public void CalculoIR_RentabilidadeZero_DeveRetornarZero()
        {
            var result = _investimentoService.CalcularIR(100, 150, TiposInvestimentoEnum.Fundos);
            Assert.Equal(0, result);
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData(829.68, 799.4720, TiposInvestimentoEnum.TesouroDireto, 3.0208)]
        [InlineData(150, 100, TiposInvestimentoEnum.Fundos, 7.50)]
        [InlineData(150, 100, TiposInvestimentoEnum.RendaFixa, 2.50)]
        [InlineData(150, 100, TiposInvestimentoEnum.TesouroDireto, 5.00)]
        [InlineData(100, 150, TiposInvestimentoEnum.TesouroDireto, 0)]
        public void CalculoIR_DeveRetornarOValorCorreto(decimal valorTotal, decimal valorInvestido, TiposInvestimentoEnum tipoInvestimento, decimal valorIR)
        {
            var result = _investimentoService.CalcularIR(valorTotal, valorInvestido, tipoInvestimento);
            Assert.Equal(valorIR, result);
        }

        public static IEnumerable<object[]> ObterDadosParaTesteCalculoValorResgate() => new List<object[]>
            {
                new object[] { 150, DateTime.Today.AddYears(-1), DateTime.Today.AddYears(5), 105 }, //Descontar 30%
                new object[] { 150, DateTime.Today.AddYears(-1), DateTime.Today.AddMonths(2), 141 }, //Descontar 6%
                new object[] { 150, DateTime.Today.AddYears(-1), DateTime.Today.AddMonths(6), 127.50 }, //Descontar 15%
                new object[] { 150, DateTime.Today.AddYears(-1), DateTime.Today, 150 }, //Não descontar nada
            };

        [Theory]
        [MemberData(nameof(ObterDadosParaTesteCalculoValorResgate))]
        public void CalculoValorResgate_DeveRetornarOValorCorreto(decimal valorTotal, DateTime dataInvestimento, DateTime dataVencimento, decimal valorResgate)
        {
            var result = _investimentoService.CalcularValorResgate(valorTotal, dataInvestimento, dataVencimento);
            Assert.Equal(valorResgate, result);
        }

        [Fact]
        public async void GetInvestimentos_DeveRetornarQuantidadeCorretaDeDados()
        {
            var result = await _investimentoService.GetInvestimentos();
            Assert.Equal(3, result.Investimentos.Count());
        }

        [Fact]
        public async void GetInvestimentos_DeveRetornarValorTotalCorreto()
        {
            var result = await _investimentoService.GetInvestimentos();
            Assert.Equal(450, result.ValorTotal);
        }
    }
}
