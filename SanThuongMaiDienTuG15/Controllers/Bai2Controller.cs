using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanThuongMaiDienTuG15.Models;

namespace SanThuongMaiDienTuG15.Controllers
{
    public class Bai2Controller : Controller
    {
        private readonly EcC2CContext _context;
        private readonly ILogger<HomeController> _logger;

        public Bai2Controller(ILogger<HomeController> logger, EcC2CContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        [HttpPost]
        public IActionResult AddCheckCode([FromBody] Product updatedProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var existingProduct = _context.Products.Find(updatedProduct.ProductId);
            if (existingProduct == null) return NotFound();

            existingProduct.ProductName = existingProduct.ProductName;
            existingProduct.Description = existingProduct.Description;
            existingProduct.CatId = existingProduct.CatId;
            existingProduct.Price = existingProduct.Price;
            existingProduct.Quantity = existingProduct.Quantity;
            existingProduct.SellerId = existingProduct.SellerId;
            existingProduct.DatePosted = existingProduct.DatePosted;
            existingProduct.ImageUrl = existingProduct.ImageUrl;
            existingProduct.ProductStatus = existingProduct.ProductStatus;
            existingProduct.Thumb = existingProduct.Thumb;



            existingProduct.CheckCode = updatedProduct.CheckCode;
            try
            {
                _context.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest("Lỗi cập nhật");
            }
        }
    }
}
