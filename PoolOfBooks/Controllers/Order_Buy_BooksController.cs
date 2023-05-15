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
    public class Order_Buy_BooksController : Controller
    {
        private readonly PoolOfBooksContext _context;

        public Order_Buy_BooksController(PoolOfBooksContext context)
        {
            _context = context;
        }

        // GET: Order_Buy_Books
        public async Task<IActionResult> Index()
        {
            var poolOfBooksContext = _context.Order_Buy_Books.Include(o => o.Books).Include(o => o.Order_Buy);
            return View(await poolOfBooksContext.ToListAsync());
        }

        // GET: Order_Buy_Books/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Order_Buy_Books == null)
            {
                return NotFound();
            }

            var order_Buy_Books = _context.Order_Buy_Books
                .Include(o => o.Books)
                .Include(o => o.Order_Buy)
                .Where(m => m.id_order == id);
            if (order_Buy_Books == null)
            {
                return NotFound();
            }

            return View(order_Buy_Books);
        }

        // GET: Order_Buy_Books/Create
        public IActionResult Create()
        {
            ViewData["id_book"] = new SelectList(_context.Books, "id", "id");
            ViewData["id_order"] = new SelectList(_context.Order_Buy, "id", "id");
            return View();
        }

        // POST: Order_Buy_Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,id_order,id_book")] Order_Buy_Books order_Buy_Books)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order_Buy_Books);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_book"] = new SelectList(_context.Books, "id", "id", order_Buy_Books.id_book);
            ViewData["id_order"] = new SelectList(_context.Order_Buy, "id", "id", order_Buy_Books.id_order);
            return View(order_Buy_Books);
        }

        // GET: Order_Buy_Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Order_Buy_Books == null)
            {
                return NotFound();
            }

            var order_Buy_Books = await _context.Order_Buy_Books.FindAsync(id);
            if (order_Buy_Books == null)
            {
                return NotFound();
            }
            ViewData["id_book"] = new SelectList(_context.Books, "id", "id", order_Buy_Books.id_book);
            ViewData["id_order"] = new SelectList(_context.Order_Buy, "id", "id", order_Buy_Books.id_order);
            return View(order_Buy_Books);
        }

        // POST: Order_Buy_Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,id_order,id_book")] Order_Buy_Books order_Buy_Books)
        {
            if (id != order_Buy_Books.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order_Buy_Books);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Order_Buy_BooksExists(order_Buy_Books.id))
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
            ViewData["id_book"] = new SelectList(_context.Books, "id", "id", order_Buy_Books.id_book);
            ViewData["id_order"] = new SelectList(_context.Order_Buy, "id", "id", order_Buy_Books.id_order);
            return View(order_Buy_Books);
        }

        // GET: Order_Buy_Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order_Buy_Books == null)
            {
                return NotFound();
            }

            var order_Buy_Books = await _context.Order_Buy_Books
                .Include(o => o.Books)
                .Include(o => o.Order_Buy)
                .FirstOrDefaultAsync(m => m.id == id);
            if (order_Buy_Books == null)
            {
                return NotFound();
            }

            return View(order_Buy_Books);
        }

        // POST: Order_Buy_Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order_Buy_Books == null)
            {
                return Problem("Entity set 'PoolOfBooksContext.Order_Buy_Books'  is null.");
            }
            var order_Buy_Books = await _context.Order_Buy_Books.FindAsync(id);
            if (order_Buy_Books != null)
            {
                _context.Order_Buy_Books.Remove(order_Buy_Books);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Order_Buy_BooksExists(int id)
        {
          return (_context.Order_Buy_Books?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
