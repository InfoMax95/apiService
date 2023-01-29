using Microsoft.AspNetCore.Mvc;

namespace apiService.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
