using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAppTemplate.Models;

namespace WebAppTemplate.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //add db
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //Home page
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        //Privacy page
        [HttpGet("[action]")]
        public IActionResult Privacy()
        {
            return View();
        }

        //Default error page used for error handling
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("[action]")]
        public IActionResult Error()
        {
            var vm = new ErrorViewModel();
            vm.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            return View(vm);
        }
    }
}