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
    public class Order_RentController : Controller
    {
        private readonly PoolOfBooksContext _context;

        public Order_RentController(PoolOfBooksContext context)
        {
            _context = context;
        }

        // GET: Order_Rent
        public async Task<IActionResult> Index()
        {
            var poolOfBooksContext = _context.Order_Rent.Include(o => o.Users).Include(o => o.RentBooks);
            return View(await poolOfBooksContext.ToListAsync());
        }

        // GET: Order_Rent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order_Rent == null)
            {
                return NotFound();
            }

            var order_Rent = await _context.Order_Rent
                .Include(o => o.Users).Include(o => o.RentBooks)
                .FirstOrDefaultAsync(m => m.id == id);
            if (order_Rent == null)
            {
                return NotFound();
            }

            return View(order_Rent);
        }

        // GET: Order_Rent/Create
        public IActionResult Create()
        {
            ViewData["id_client"] = new SelectList(_context.Users, "id", "login");
            ViewData["status"] = new SelectList(new List<string> { "Создан" }, "Создан");
            ViewData["cart"] = new List<Cart>(_context.Cart.Where(x => x.userId == Convert.ToInt32(User.FindFirst("ID").Value)));
            return View();
        }

        // POST: Order_Rent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,id_client,date_begin,date_end,sum,address,status")] Order_Rent order_Rent)
        {
                _context.Add(order_Rent);
                await _context.SaveChangesAsync();
                return Redirect($"~/Users/Details/{order_Rent.id_client}");
        }

        // GET: Order_Rent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Order_Rent == null)
            {
                return NotFound();
            }

            var order_Rent = await _context.Order_Rent.FindAsync(id);
            if (order_Rent == null)
            {
                return NotFound();
            }
            ViewData["id_client"] = new SelectList(_context.Users, "id", "login" ,order_Rent.id_client);

            ViewData["status"] = new SelectList(new List<string> { "Создан", "Собран", "Доставлен", "Получен", "В аренде", "Выполнен", "Продлен" }, order_Rent.status);

            return View(order_Rent);
        }

        // POST: Order_Rent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,id_client,date_begin,date_end,sum,address,status")] Order_Rent order_Rent)
        {
            if (id != order_Rent.id)
            {
                return NotFound();
            }

            
                try
                {
                    _context.Update(order_Rent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Order_RentExists(order_Rent.id))
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

        // GET: Order_Rent/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order_Rent == null)
            {
                return NotFound();
            }

            var order_Rent = await _context.Order_Rent
                .Include(o => o.Users)
                .FirstOrDefaultAsync(m => m.id == id);
            if (order_Rent == null)
            {
                return NotFound();
            }

            return View(order_Rent);
        }

        // POST: Order_Rent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order_Rent == null)
            {
                return Problem("Entity set 'PoolOfBooksContext.Order_Rent'  is null.");
            }
            var order_Rent = await _context.Order_Rent.FindAsync(id);
            if (order_Rent != null)
            {
                _context.Order_Rent.Remove(order_Rent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Order_RentExists(int id)
        {
          return (_context.Order_Rent?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
