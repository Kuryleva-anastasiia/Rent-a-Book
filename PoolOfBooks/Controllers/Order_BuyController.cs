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
            var poolOfBooksContext = _context.Order_Buy.Include(o => o.Users);
            return View(await poolOfBooksContext.ToListAsync());
        }

        // GET: Order_Buy/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order_Buy == null)
            {
                return NotFound();
            }

            var order_Buy = await _context.Order_Buy
                .Include(o => o.Users)
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
            ViewData["id_client"] = new SelectList(_context.Users, "id", "login");
            return View();
        }

        // POST: Order_Buy/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,id_client,date,sum,address,status")] Order_Buy order_Buy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order_Buy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_client"] = new SelectList(_context.Users, "id", "login", order_Buy.id_client);
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
            ViewData["id_client"] = new SelectList(_context.Users, "id", "login", order_Buy.id_client);
            ViewData["status"] = new SelectList(new List<string> { "Создан", "Собран", "Доставлен", "Получен", "В аренде", "Выполнен", "Продлен" }, order_Buy.status);
            return View(order_Buy);
        }

        // POST: Order_Buy/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,id_client,date,sum,address,status")] Order_Buy order_Buy)
        {
            if (id != order_Buy.id)
            {
                return NotFound();
            }

            
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

        // GET: Order_Buy/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order_Buy == null)
            {
                return NotFound();
            }

            var order_Buy = await _context.Order_Buy
                .Include(o => o.Users)
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
