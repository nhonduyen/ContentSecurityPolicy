using CFS.Models;
using Microsoft.AspNetCore.Mvc;

namespace CFS.Controllers
{
    public class CSPController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public CSPController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CSPReport([FromBody] CspReportRequest cspReportRequest)
        {
            _logger.LogWarning(@$"CSP Violation: {cspReportRequest.CspReport.DocumentUri}, {cspReportRequest.CspReport.BlockedUri}");
            return Ok();
        }
    }
}
