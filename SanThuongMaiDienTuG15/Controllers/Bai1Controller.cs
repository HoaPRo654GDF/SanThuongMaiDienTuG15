using Microsoft.AspNetCore.Mvc;
using SanThuongMaiDienTuG15.Models;

namespace SanThuongMaiDienTuG15.Controllers
{
    public class Bai1Controller : Controller
    {
        private readonly EcC2CContext _context;
      

        public Bai1Controller( EcC2CContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
        [HttpGet]
        public JsonResult GetProduct(int id)
        {
            var product = _context.Products.Find(id);
            return Json(product);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Product product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingProduct = _context.Products.Find(product.ProductId);
            if (existingProduct == null) return NotFound();

            // Giữ lại các trường c
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

            // Cập nhật VerifyKey mới
            existingProduct.VerifyKey = product.VerifyKey;

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

        [HttpPost]
        public IActionResult Edit([FromBody] Product updatedProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingProduct = _context.Products.Find(updatedProduct.ProductId);
            if (existingProduct == null) return NotFound();

            //existingProduct.ProductName = updatedProduct.ProductName;
            //existingProduct.Description = updatedProduct.Description;
            existingProduct.VerifyKey = updatedProduct.VerifyKey;

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


        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
