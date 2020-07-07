using Investimentos.Core;
using Investimentos.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Investimentos.Api.Controllers
{
    [ApiController]
    [Route("investimentos")]
    public class InvestimentoController : ControllerBase
    {
        private const string _getInvestimentoChaveCache = "aa84cb73-c4ec-4fca-8319-3814909bad7c";
        private readonly IMemoryCache _memoryCache;
        private readonly InvestimentoService _investimentoService;

        public InvestimentoController(
            IMemoryCache memoryCache,
            InvestimentoService investimentoService)
        {
            _memoryCache = memoryCache;
            _investimentoService = investimentoService;
        }

        [HttpGet]
        public async Task<GetInvestimentoResult> Get()
        {
            if (!_memoryCache.TryGetValue(_getInvestimentoChaveCache, out GetInvestimentoResult result))
            {
                result = await _investimentoService.GetInvestimentos();
                _memoryCache.Set(_getInvestimentoChaveCache, result, DateTime.Now.AddMinutes(2));
            }

            return result;
        }
    }
}