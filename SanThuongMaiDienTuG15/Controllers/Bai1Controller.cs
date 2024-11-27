using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        //[Authorize(Roles = "2")]
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Accounts");
            }

            // Kiểm tra role
            if (!User.IsInRole("2"))
            {
                return RedirectToAction("Index", "Home");  
            }
            var sellerId = User.FindFirst("UserID")?.Value;
            if (!int.TryParse(sellerId, out int userId))
            {
                return Unauthorized();
            }

            var products = _context.Products.Where(p => p.SellerId == userId).ToList();
            ViewBag.Categories = _context.Categories.ToList();
            //var products = _context.Products.ToList();
            return View(products);
        }
        [HttpGet]
        public JsonResult GetProduct(int id)
        {

            var sellerId = int.Parse(User.FindFirst("UserID").Value);
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id && p.SellerId == sellerId);

            //var product = _context.Products.Find(id);
            return Json(product);
        }

        [Authorize(Roles = "2")]
        [HttpPost]
        public IActionResult Create([FromForm] int productId, [FromForm] string verifyKey)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var sellerId = int.Parse(User.FindFirst("UserID").Value);
            var existingProduct = _context.Products.FirstOrDefault(
                p => p.ProductId == productId && p.SellerId == sellerId);

            if (existingProduct == null) return NotFound();

            existingProduct.VerifyKey = verifyKey;
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

        [Authorize(Roles = "2")]
        [HttpPost]
        public IActionResult Edit([FromForm] int productId, [FromForm] string verifyKey)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sellerId = int.Parse(User.FindFirst("UserID").Value);
            var existingProduct = _context.Products.FirstOrDefault(
                p => p.ProductId == productId && p.SellerId == sellerId);

            if (existingProduct == null) return NotFound();

            existingProduct.VerifyKey = verifyKey;
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

        [Authorize(Roles = "2")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var sellerId = int.Parse(User.FindFirst("UserID").Value);
            var product = await _context.Products.FirstOrDefaultAsync(
                p => p.ProductId == id && p.SellerId == sellerId);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = "2")]
        [HttpPost]
        public async Task<IActionResult> AddNew([FromForm] Product newProduct, IFormFile thumbFile)
        {
            try
            {
                if (thumbFile == null)
                    return BadRequest("Vui lòng chọn ảnh sản phẩm");

                var sellerId = int.Parse(User.FindFirst("UserID").Value);
                newProduct.SellerId = sellerId;
                newProduct.DatePosted = DateTime.Now;
                newProduct.ProductStatus = newProduct.Quantity > 0 ? "Còn hàng" : "Hết hàng";

                // Xử lý upload ảnh
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images/product");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + thumbFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await thumbFile.CopyToAsync(stream);
                }

                newProduct.Thumb = uniqueFileName;

                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();
                return Ok(newProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }
    }

}
