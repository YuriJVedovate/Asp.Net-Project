using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace ASPCORE.Controllers
{
  public class FirstController : Controller
  {

    [HttpGet("toview")]
    public IActionResult ToView()
    {
      return View();
    }

    [HttpPost("dataform")]
    public IActionResult DataForm()
    {
      Request.Form.TryGetValue("name", out StringValues name);
      Request.Form.TryGetValue("email", out StringValues email);
      return Content("form send! \nname: " + name + "\nEmail: " + email);
    }

    // [HttpGet("test/{number:int?}")]
    // public IActionResult index(int number)
    // {
    //   return Content("the number is: " + number);
    // }


    // [HttpGet("more")]
    // public IActionResult more()
    // {
    //   var name = Request.Query["name"];
    //   return Content(name + " is learn Asp.NET core");
    // }
  }
}