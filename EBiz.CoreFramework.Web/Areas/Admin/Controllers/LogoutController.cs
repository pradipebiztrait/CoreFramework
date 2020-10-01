using Microsoft.AspNetCore.Mvc;

namespace EBiz.CoreFramework.Web.AdminControllers
{
    [Area("Admin")]
    public class LogoutController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login", new { area="Admin"});
        }
    }
}