using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  //derive from controller because will be provided as a View 
  public class FallbackController : Controller
  {
    public IActionResult Index()
    {
      return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"),"text/HTML");
    }
  }
}