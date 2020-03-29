using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ServiceCommodity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommodityController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "啤酒", "花生", "烤鸭", "烤串"
        };

        private readonly ILogger<CommodityController> _logger;

        public CommodityController(ILogger<CommodityController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return Summaries;
        }
    }
}
