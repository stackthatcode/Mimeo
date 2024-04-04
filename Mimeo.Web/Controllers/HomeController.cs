using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mimeo.Middle.Identity;

namespace Mimeo.Web.Controllers
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
        public IActionResult Laboratory()
        {
            return View();
        }

        public IActionResult Throw()
        {
            throw new Exception("sdgkds;gkl;kgh;k!");
        }

        [Authorize(Policy = "AdminsOnly")]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
