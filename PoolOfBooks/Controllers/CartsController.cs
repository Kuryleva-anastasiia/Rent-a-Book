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
    public class CartsController : Controller
    {
        private readonly PoolOfBooksContext _context;

        public CartsController(PoolOfBooksContext context)
        {
            _context = context;
        }
        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var poolOfBooksContext = _context.Cart.Include(c => c.Books).Include(c => c.Users);
            return View(await poolOfBooksContext.ToListAsync());
        }

        // GET: Carts/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = _context.Cart
                .Include(c => c.Books)
                .Include(c => c.Users).Where(x => x.id_client == id);

            if (cart == null)
            {
                return NotFound();
            }

            return View(cart.ToListAsync());
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["id_book"] = new SelectList(_context.Books, "id", "id");
            ViewData["id_client"] = new SelectList(_context.Users, "id", "id");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,id_client,id_book,status")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_book"] = new SelectList(_context.Books, "id", "id", cart.id_book);
            ViewData["id_client"] = new SelectList(_context.Users, "id", "id", cart.id_client);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["id_book"] = new SelectList(_context.Books, "id", "id", cart.id_book);
            ViewData["id_client"] = new SelectList(_context.Users, "id", "id", cart.id_client);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,id_client,id_book,status")] Cart cart)
        {
            if (id != cart.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.id))
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
            ViewData["id_book"] = new SelectList(_context.Books, "id", "id", cart.id_book);
            ViewData["id_client"] = new SelectList(_context.Users, "id", "id", cart.id_client);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Books)
                .Include(c => c.Users)
                .FirstOrDefaultAsync(m => m.id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cart == null)
            {
                return Problem("Entity set 'PoolOfBooksContext.Cart'  is null.");
            }
            var cart = await _context.Cart.FindAsync(id);
            if (cart != null)
            {
                _context.Cart.Remove(cart);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
          return (_context.Cart?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
