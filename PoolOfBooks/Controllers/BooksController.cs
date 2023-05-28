using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using PoolOfBooks.Data;
using PoolOfBooks.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace PoolOfBooks.Controllers
{
    public class BooksController : Controller
    {
        private readonly PoolOfBooksContext _context;
        private readonly INotyfService _toastNotification;
        public BooksController(PoolOfBooksContext context, INotyfService toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }


        // GET: Books
        public async Task<IActionResult> Index(SortState sortOrder = SortState.NameAsc)
        {
            ViewData["NameSort"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["AuthorSort"] = sortOrder == SortState.AuthorAsc ? SortState.AuthorDesc : SortState.AuthorAsc;
            ViewData["PriceSort"] = sortOrder == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;
            ViewData["StatusSort"] = sortOrder == SortState.StatusRent ? SortState.StatusBuy : SortState.StatusRent;

            if (_context.Books != null)
            {
                IQueryable<Books>? books = _context.Books.Include(o => o.RentBooks).Include(o => o.Carts).Include(o => o.Category);

                books = sortOrder switch
                {
                    SortState.NameDesc => books.OrderByDescending(s => s.name),
                    SortState.AuthorAsc => books.OrderBy(s => s.author),
                    SortState.AuthorDesc => books.OrderByDescending(s => s.author),
                    SortState.PriceAsc => books.OrderBy(s => s.price),
                    SortState.PriceDesc => books.OrderByDescending(s => s.price),
                    SortState.StatusRent => books.Where(s => s.status == "аренда"),
                    SortState.StatusBuy => books.Where(s => s.status == "продажа"),
                    _ => books.OrderBy(s => s.name),
                };

                return View(await books.ToListAsync());

            }
            else
            {
                Problem("Entity set 'PoolOfBooksContext.Books'  is null.");
            }

            return View();

        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books.Include(o => o.Category)
                .FirstOrDefaultAsync(m => m.id == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["id_category"] = new SelectList(_context.Category, "id", "name");
            ViewData["cover"] = new SelectList(new List<string> { "твердая", "мягкая" }, "твердая");
            ViewData["status"] = new SelectList(new List<string> { "аренда", "продажа" }, "аренда");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,author,cycle,description,id_category,cover,pages,in_stock,price,status,count_was_read,buyPrice,sellPrice")] Books books)
        {
            try
            {
                if (books.buyPrice <= 500)
                {
                    books.price = 150;
                }
                else
                {
                    if (books.buyPrice <= 800)
                    {
                        books.price = 200;
                    }
                    else
                    {
                        books.price = 250;
                    }
                }

                books.sellPrice = books.buyPrice / 2;

                _context.Books.Add(books);
                await _context.SaveChangesAsync();
                _toastNotification.Success("Книга добавлена!\n", 10);
                return Redirect("~/Books/Index");
            }
            catch (Exception ex) { _toastNotification.Error("Ошибка!\n" + ex.Message, 10);
                return View(books);
            }
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books.FindAsync(id);
            if (books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.Include(o => o.Category)
                .FirstOrDefaultAsync(m => m.id == id);

            if (book == null)
            {
                return NotFound();
            }

            ViewData["id_category"] = new SelectList(_context.Category, "id", "name", book.Category.id);
            ViewData["cover"] = new SelectList(new List<string> { "твердая", "мягкая" }, books.cover);
            ViewData["status"] = new SelectList(new List<string> { "аренда", "продажа" }, books.status);
            return View(books);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,author,cycle,description,id_category,cover,pages,in_stock,price,status,count_was_read,buyPrice,sellPrice")] Books books)
        {
            if (id != books.id)
            {
                return NotFound();
            }

            try
            {
                books.sellPrice = books.buyPrice / 2;

                if (books.buyPrice <= 500)
                {
                    books.price = 150;
                }
                else
                {
                    if (books.buyPrice <= 800)
                    {
                        books.price = 200;
                    }
                    else
                    {
                        books.price = 250;
                    }
                }

                _context.Update(books);
                await _context.SaveChangesAsync();
                _toastNotification.Success("Информация изменена успешно!\n", 10);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!BooksExists(books.id))
                {
                    _toastNotification.Error("Ошибка!\n" + ex.Message, 10);
                    return NotFound();
                }
                else
                {
                    _toastNotification.Error("Ошибка!\n" + ex.Message, 10);
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .FirstOrDefaultAsync(m => m.id == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'PoolOfBooksContext.Books'  is null.");
            }
            var books = await _context.Books.FindAsync(id);
            if (books != null)
            {
                _context.Books.Remove(books);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BooksExists(int id)
        {
          return (_context.Books?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
