using Microsoft.AspNetCore.Mvc;

namespace SanThuongMaiDienTuG15.Areas.Seller.Controllers
{
    public class ContentController : Controller
    {
        public IActionResult NameUser()
        {
            return ViewComponent("NameUser");
        }
    }
}
