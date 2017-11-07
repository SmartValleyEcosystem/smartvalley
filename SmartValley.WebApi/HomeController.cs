using Microsoft.AspNetCore.Mvc;

namespace SmartValley.WebApi
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return File("index.html", "text/html");
        }
    }
}