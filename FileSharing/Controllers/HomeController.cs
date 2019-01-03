using Microsoft.AspNetCore.Mvc;

namespace FileSharing.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "File");
        }
    }
}