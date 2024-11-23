using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SanThuongMaiDienTuG15.Areas.Seller.Controllers
{
    public class HomeController : Controller
    {
        [Area("Seller")]
        [Authorize(Roles = "2")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
