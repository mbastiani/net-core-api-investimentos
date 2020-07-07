using Investimentos.Core;
using Investimentos.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Investimentos.Api.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Ok";
        }
    }
}