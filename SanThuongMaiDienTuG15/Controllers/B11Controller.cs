
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanThuongMaiDienTuG15.Models;

[Authorize(Roles = "2")]
public class B11Controller : Controller
{
    private readonly EcC2CContext _context;

    public B11Controller(EcC2CContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Accounts");
        }

        var userIdClaim = User.FindFirst("UserID");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int sellerId))
        {
            return RedirectToAction("Login", "Accounts");
        }

        var products = _context.Products.Where(p => p.SellerId == sellerId).ToList();
        return View(products);
    }

    [HttpPost]
    public IActionResult Create([FromForm] Product product)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }

            var existingProduct = _context.Products.Find(product.ProductId);
            if (existingProduct == null)
            {
                return BadRequest("Không tìm thấy sản phẩm");
            }


          
            existingProduct.VerifyKey = product.VerifyKey;
            _context.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
           
            return BadRequest($"Lỗi: {ex.Message}");
        }
    }

    [HttpPost]
    public IActionResult Edit([FromForm] Product product)
    {
        var sellerId = int.Parse(User.FindFirst("UserID").Value);
        var existingProduct = _context.Products.FirstOrDefault(
            p => p.ProductId == product.ProductId && p.SellerId == sellerId);

        if (existingProduct == null) return NotFound();

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

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var sellerId = int.Parse(User.FindFirst("UserID").Value);
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.ProductId == id && p.SellerId == sellerId);

        if (product == null) return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
