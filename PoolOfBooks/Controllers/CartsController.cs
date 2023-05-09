using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
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
        private readonly INotyfService _toastNotification;
        public CartsController(PoolOfBooksContext context, INotyfService toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public IActionResult Notify()
        {
            var id = User.FindFirst("ID").Value;
            _toastNotification.Warning("Книга удалена!");
            return Redirect($"~/carts/details/{id}");
        }

        public IActionResult CartAddNotify()
        {
            var cart = _context.Cart.OrderBy(x => x.id).Last();
            _toastNotification.Success("Книга добавлена в корзину!");
            return Redirect($"~/Books/index#{cart.bookId}");
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
                .Include(c => c.Users).Where(x => x.Users.id == id).ToList();
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["bookId"] = new SelectList(_context.Books, "id", "name");
            ViewData["userId"] = new SelectList(_context.Users, "id", "first_name");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,userId,bookId,status")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["bookId"] = new SelectList(_context.Books, "id", "id", cart.bookId);
            ViewData["userId"] = new SelectList(_context.Users, "id", "id", cart.userId);
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
            ViewData["bookId"] = new SelectList(_context.Books, "id", "id", cart.bookId);
            ViewData["userId"] = new SelectList(_context.Users, "id", "id", cart.userId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,userId,bookId,status")] Cart cart)
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
            ViewData["bookId"] = new SelectList(_context.Books, "id", "id", cart.bookId);
            ViewData["userId"] = new SelectList(_context.Users, "id", "id", cart.userId);
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
        public async Task<IActionResult> DeleteConfirmed(int id, string? returnUrl)
        {
            if (_context.Cart == null)
            {
                return Problem("Entity set 'PoolOfBooksContext.Cart'  is null.");
            }
            var cart = await _context.Cart.FindAsync(id);
            var userId = User.FindFirst("ID").Value;
            if (cart != null)
            {
                _context.Cart.Remove(cart);
            }
            
            await _context.SaveChangesAsync();
            return Redirect(returnUrl ?? $"~/Carts/Details/{userId}");
        }

        private bool CartExists(int id)
        {
          return (_context.Cart?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
