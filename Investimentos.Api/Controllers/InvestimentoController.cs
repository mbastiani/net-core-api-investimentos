using Investimentos.Domain.DTOs;
using Investimentos.Infra.Exceptions;
using Investimentos.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Investimentos.Api.Controllers
{
    [ApiController]
    [Route("investimentos")]
    public class InvestimentoController : ControllerBase
    {
        private const string _getInvestimentoChaveCache = "aa84cb73-c4ec-4fca-8319-3814909bad7c";
        private readonly ILogger<InvestimentoController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly InvestimentoService _investimentoService;

        public InvestimentoController(
            IMemoryCache memoryCache,
            InvestimentoService investimentoService,
            ILogger<InvestimentoController> logger)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _investimentoService = investimentoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                if (!_memoryCache.TryGetValue(_getInvestimentoChaveCache, out GetInvestimentoResult result))
                {
                    result = await _investimentoService.GetInvestimentos();
                    _memoryCache.Set(_getInvestimentoChaveCache, result, DateTime.Today.AddDays(1));
                }

                return Ok(result);
            }
            catch(ApiException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ErroResult { Mensagem = ex.Message });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Erro interno");
                return StatusCode(500, new ErroResult { Mensagem = "Erro interno" });
            }
        }
    }
}