using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SanThuongMaiDienTuG15.Models;

namespace SanThuongMaiDienTuG15.Areas.Baithi.Controllers
{
    [Area("Baithi")]
    public class BaithiProductsController : Controller
    {
        private readonly EcC2CContext _context;

        public BaithiProductsController(EcC2CContext context)
        {
            _context = context;
        }

        // GET: Baithi/BaithiProducts
        public async Task<IActionResult> Index()
        {
            var ecC2CContext = _context.Products.Include(p => p.Cat).Include(p => p.Seller);
            return View(await ecC2CContext.ToListAsync());
        }

        // GET: Baithi/BaithiProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cat)
                .Include(p => p.Seller)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Baithi/BaithiProducts/Create
        public IActionResult Create()
        {
            ViewData["CatId"] = new SelectList(_context.Categories, "CatId", "CatId");
            ViewData["SellerId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Baithi/BaithiProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,Description,CatId,Price,Quantity,SellerId,DatePosted,ImageUrl,ProductStatus,Thumb")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatId"] = new SelectList(_context.Categories, "CatId", "CatId", product.CatId);
            ViewData["SellerId"] = new SelectList(_context.Users, "UserId", "UserId", product.SellerId);
            return View(product);
        }

        // GET: Baithi/BaithiProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CatId"] = new SelectList(_context.Categories, "CatId", "CatId", product.CatId);
            ViewData["SellerId"] = new SelectList(_context.Users, "UserId", "UserId", product.SellerId);
            return View(product);
        }

        // POST: Baithi/BaithiProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,Description,CatId,Price,Quantity,SellerId,DatePosted,ImageUrl,ProductStatus,Thumb")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatId"] = new SelectList(_context.Categories, "CatId", "CatId", product.CatId);
            ViewData["SellerId"] = new SelectList(_context.Users, "UserId", "UserId", product.SellerId);
            return View(product);
        }

        // GET: Baithi/BaithiProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cat)
                .Include(p => p.Seller)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Baithi/BaithiProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'EcC2CContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
