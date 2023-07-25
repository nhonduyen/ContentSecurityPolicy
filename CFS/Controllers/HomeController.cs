using CFS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CFS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CSPReport([FromBody] CspReportRequest cspReportRequest)
        {
            _logger.LogWarning(@$"CSP Violation: {cspReportRequest.CspReport.DocumentUri}, {cspReportRequest.CspReport.BlockedUri}"); 
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}