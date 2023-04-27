using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PoolOfBooks.Data;
using PoolOfBooks.Models;

namespace PoolOfBooks.Controllers
{
    public class Order_BuyController : Controller
    {
        private readonly PoolOfBooksContext _context;

        public Order_BuyController(PoolOfBooksContext context)
        {
            _context = context;
        }

        // GET: Order_Buy
        public async Task<IActionResult> Index()
        {
              return _context.Order_Buy != null ? 
                          View(await _context.Order_Buy.ToListAsync()) :
                          Problem("Entity set 'PoolOfBooksContext.Order_Buy'  is null.");
        }

        // GET: Order_Buy/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order_Buy == null)
            {
                return NotFound();
            }

            var order_Buy = await _context.Order_Buy
                .FirstOrDefaultAsync(m => m.id == id);
            if (order_Buy == null)
            {
                return NotFound();
            }

            return View(order_Buy);
        }

        // GET: Order_Buy/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Order_Buy/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,id_client,date,sum,address")] Order_Buy order_Buy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order_Buy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order_Buy);
        }

        // GET: Order_Buy/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Order_Buy == null)
            {
                return NotFound();
            }

            var order_Buy = await _context.Order_Buy.FindAsync(id);
            if (order_Buy == null)
            {
                return NotFound();
            }
            return View(order_Buy);
        }

        // POST: Order_Buy/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,id_client,date,sum,address")] Order_Buy order_Buy)
        {
            if (id != order_Buy.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order_Buy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Order_BuyExists(order_Buy.id))
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
            return View(order_Buy);
        }

        // GET: Order_Buy/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order_Buy == null)
            {
                return NotFound();
            }

            var order_Buy = await _context.Order_Buy
                .FirstOrDefaultAsync(m => m.id == id);
            if (order_Buy == null)
            {
                return NotFound();
            }

            return View(order_Buy);
        }

        // POST: Order_Buy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order_Buy == null)
            {
                return Problem("Entity set 'PoolOfBooksContext.Order_Buy'  is null.");
            }
            var order_Buy = await _context.Order_Buy.FindAsync(id);
            if (order_Buy != null)
            {
                _context.Order_Buy.Remove(order_Buy);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Order_BuyExists(int id)
        {
          return (_context.Order_Buy?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
