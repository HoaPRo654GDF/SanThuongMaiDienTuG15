using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SanThuongMaiDienTuG15.Models;

namespace SanThuongMaiDienTuG15.Areas.Baithi.Controllers
{
    [Area("Baithi")]
    public class HomeController : Controller
    {
        private readonly EcC2CContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(EcC2CContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == 26);
            //if (product == null)
            //{
            //    _logger.LogWarning("Product with ID 22 not found");
               
            //}
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProduct([FromBody] DeleteProductRequest request)
        {
            _logger.LogInformation($"Received delete request for product ID: {request?.id}");

            if (request?.id == null)
            {
                _logger.LogWarning("Delete request received with null ID");
                return Json(new { success = false, message = "ID sản phẩm không hợp lệ." });
            }

            try
            {
                // Log the current product in database
                var existingProduct = _context.Products.FirstOrDefault(p => p.ProductId == request.id);
                if (existingProduct == null)
                {
                    _logger.LogWarning($"Product with ID {request.id} not found in database");
                    return Json(new { success = false, message = "Sản phẩm không tồn tại." });
                }

                _logger.LogInformation($"Found product: {existingProduct.ProductName} with ID: {existingProduct.ProductId}");

                _context.Products.Remove(existingProduct);
                _context.SaveChanges();

                _logger.LogInformation($"Successfully deleted product with ID: {request.id}");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting product {request.id}: {ex.Message}");
                return Json(new { success = false, message = $"Lỗi khi xóa sản phẩm: {ex.Message}" });
            }
        }
    }

    public class DeleteProductRequest
    {
        public int id { get; set; }
    }
}