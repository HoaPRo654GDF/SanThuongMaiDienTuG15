using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanThuongMaiDienTuG15.ModelViews;
using System.Collections.Generic;
using System;

namespace SanThuongMaiDienTuG15.Areas.Seller.Controllers.Components
{
    public class NameUserViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var userName = User.Identity.Name;
            return View("Default", userName);
        }
    }
}
