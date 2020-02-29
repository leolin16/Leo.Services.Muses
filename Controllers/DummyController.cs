using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Leo.Services.Muses.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Leo.Services.Muses.Controllers
{
    [Route("api/[controller]")]
    public class DummyController : Controller
    {
        private MusesDbContext _ctx;
        private ILogger<DummyController> _logger;
        public DummyController(MusesDbContext ctx, ILogger<DummyController> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }
        [HttpGet]
        [Route("")]
        public IActionResult TestDatabase() {
            _logger.LogInformation("app.db might be created");
            return Ok();
        }
    }
}
