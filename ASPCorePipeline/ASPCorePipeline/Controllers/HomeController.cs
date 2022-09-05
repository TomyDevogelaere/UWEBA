using Microsoft.AspNetCore.Mvc;

namespace ASPCorePipeline.Controllers;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("It works!");
        }

    public IActionResult Crash()
    {
        //lets emulate a runtime exeption
        int z = 0;
        return Ok(10 / z);
    }
    public IActionResult Error()
        => Content("Tes iet nie just!");
 

    }

