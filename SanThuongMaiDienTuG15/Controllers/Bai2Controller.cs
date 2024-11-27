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
        //public IActionResult Index()
        //{
        //    var products = _context.Products.ToList();
        //    return View(products);
        //}

        //[HttpPost]
        //public IActionResult AddCheckCode([FromBody] Product updatedProduct)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);
        //    var existingProduct = _context.Products.Find(updatedProduct.ProductId);
        //    if (existingProduct == null) return NotFound();

        //    existingProduct.ProductName = existingProduct.ProductName;
        //    existingProduct.Description = existingProduct.Description;
        //    existingProduct.CatId = existingProduct.CatId;
        //    existingProduct.Price = existingProduct.Price;
        //    existingProduct.Quantity = existingProduct.Quantity;
        //    existingProduct.SellerId = existingProduct.SellerId;
        //    existingProduct.DatePosted = existingProduct.DatePosted;
        //    existingProduct.ImageUrl = existingProduct.ImageUrl;
        //    existingProduct.ProductStatus = existingProduct.ProductStatus;
        //    existingProduct.Thumb = existingProduct.Thumb;



        //    existingProduct.CheckCode = updatedProduct.CheckCode;
        //    try
        //    {
        //        _context.SaveChanges();
        //        return Ok();
        //    }
        //    catch
        //    {
        //        return BadRequest("Lỗi cập nhật");
        //    }
        //}

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            var categories = _context.Categories.ToList();
            var sellers = _context.Users.Where(u => u.RoleId == 2).ToList();

            ViewBag.Categories = categories;
            ViewBag.Sellers = sellers;
            return View(products);
        }




        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Product newProduct, IFormFile thumbFile)
        {
            if (thumbFile != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images/product");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + thumbFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await thumbFile.CopyToAsync(fileStream);
                }
                newProduct.Thumb = uniqueFileName;
            }

            newProduct.DatePosted = DateTime.Now;
            newProduct.ProductStatus = newProduct.Quantity > 0 ? "Còn hàng" : "Hết hàng";

            try
            {
                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }
    }
}
