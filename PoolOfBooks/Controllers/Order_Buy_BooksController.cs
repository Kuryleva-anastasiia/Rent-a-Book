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
              return _context.Order_Buy_Books != null ? 
                          View(await _context.Order_Buy_Books.ToListAsync()) :
                          Problem("Entity set 'PoolOfBooksContext.Order_Buy_Books'  is null.");
        }

        // GET: Order_Buy_Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order_Buy_Books == null)
            {
                return NotFound();
            }

            var order_Buy_Books = await _context.Order_Buy_Books
                .FirstOrDefaultAsync(m => m.id == id);
            if (order_Buy_Books == null)
            {
                return NotFound();
            }

            return View(order_Buy_Books);
        }

        // GET: Order_Buy_Books/Create
        public IActionResult Create()
        {
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
