using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SanThuongMaiDienTuG15.Models;

namespace SanThuongMaiDienTuG15.Areas.Baithi.Controllers
{
    [Area("Baithi")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly EcC2CContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;
        public INotyfService _notifyService { get; }

        public HomeController(EcC2CContext context, ILogger<HomeController> logger, IWebHostEnvironment environment, INotyfService notifyService)
        {
            _context = context;
            _logger = logger;
            _environment = environment;
            _notifyService = notifyService;
        }

        public IActionResult Index()
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == 36);
            
            return View(product);
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _context.Categories.Select(c => new { c.CatId, c.CatName }).ToList();
            return Ok(categories);
        }

        [HttpPut]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Edit(int id, [FromForm] Product updatedProduct, IFormFile? thumbFile)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.ProductName = updatedProduct.ProductName;
                product.Price = updatedProduct.Price;
                product.Description = updatedProduct.Description;
                product.DatePosted = updatedProduct.DatePosted;
                product.Quantity = updatedProduct.Quantity;
                product.ProductStatus = updatedProduct.Quantity > 0 ? "Còn hàng" : "Hết hàng";
                product.CatId = updatedProduct.CatId;
                product.ImageUrl = updatedProduct.ImageUrl;

                if (thumbFile != null)
                {
                    // Xử lý upload ảnh mới
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "assets", "images", "product");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + thumbFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await thumbFile.CopyToAsync(fileStream);
                    }

                    // Cập nhật tên file ảnh mới
                    product.Thumb = uniqueFileName;
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
            return View("Index");
        }

        [HttpPost]
        [Authorize(Roles = "2")] // Chỉ cho phép Seller thêm sản phẩm
        public async Task<IActionResult> Add([FromForm] Product newProduct, IFormFile? thumbFile)
        {
            try
            {
                // Lấy ID người dùng từ Claims
                var userIdClaim = User.FindFirst("UserID")?.Value;
                if (!int.TryParse(userIdClaim, out int sellerId))
                {
                    return Unauthorized();
                }

                // Thiết lập thông tin sản phẩm
                newProduct.SellerId = sellerId;
                newProduct.DatePosted = DateTime.Now;
                newProduct.ProductStatus = newProduct.Quantity > 0 ? "Còn hàng" : "Hết hàng";

                // Xử lý upload ảnh
                if (thumbFile != null)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "assets", "images", "product");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + thumbFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await thumbFile.CopyToAsync(fileStream);
                    }
                    newProduct.Thumb = uniqueFileName;
                }

                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();

                _notifyService.Success("Thêm sản phẩm thành công!");
                return Ok(newProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm sản phẩm mới");
                _notifyService.Error("Có lỗi xảy ra khi thêm sản phẩm!");
                return BadRequest("Có lỗi xảy ra khi thêm sản phẩm");
            }
        }

        //[HttpPut]
        //public IActionResult Edit(int id, Product updatedProduct)
        //{
        //    var product = _context.Products.FirstOrDefault(x => x.ProductId == id);
        //    if (product != null)
        //    {
        //        product.ProductName = updatedProduct.ProductName;
        //        product.Price = updatedProduct.Price;
        //        product.Description = updatedProduct.Description;
        //        product.Quantity = updatedProduct.Quantity;
        //        product.ProductStatus = updatedProduct.ProductStatus;
        //        product.CatId = updatedProduct.CatId;

        //        _context.SaveChanges();
        //        return Ok();
        //    }
        //    return View("Index");
        //}

        [HttpDelete]
        [Authorize(Roles = "2")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.ProductId == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return View("Index");
        }

    }
}