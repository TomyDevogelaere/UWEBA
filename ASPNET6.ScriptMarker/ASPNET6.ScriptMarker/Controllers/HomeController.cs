using ASPNET6.ScriptMarker.Models;
using ASPNET6.ScriptMarker.Utils;
using ASPNET6.ScriptMarker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASPNET6.ScriptMarker.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger) => this._logger = logger;

    [HttpPost]
    public ActionResult MarkPdf(MarkPdfViewModel vm)
    {
      if (vm.Name is null)
      {
        return BadRequest("Name is mandatory");
      }
      if (vm.PostedFile is null)
      {
        return View(viewName: "Error", new ErrorViewModel
        {
          RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
      }

      //get the pdf file from the request
      var inputStream = vm.PostedFile.OpenReadStream();

      MemoryStream output = new();
      byte[] result = ((MemoryStream)output.WriteWatermarkOnPdf(vm.Name,
                                             inputStream)).ToArray();
      return File(result, "application/pdf");
    }

    public IActionResult Index() => View();

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}