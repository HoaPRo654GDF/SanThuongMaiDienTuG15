using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SanThuongMaiDienTuG15.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        [Authorize(Roles = "3")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
