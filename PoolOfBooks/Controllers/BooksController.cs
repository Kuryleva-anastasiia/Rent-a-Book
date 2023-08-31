using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PoolOfBooks.Data;
using PoolOfBooks.Models;

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
        public async Task<IActionResult> Index(string? name, string? category, string? sort, SortState sortOrder = SortState.NameAsc)
        {

            if (_context.Books != null)
            {
                IQueryable<Books>? books = _context.Books.Include(o => o.RentBooks).Include(o => o.Carts).Include(o => o.Category);

                //сортировка по названию, автору, цене и статусу
                // сортировка
                switch (sort)
                {
                    case "По названию А-Я":
                        books = books.OrderBy(s => s.name);
                        break;
                    case "По названию Я-А":
                        books = books.OrderByDescending(s => s.name);
                        break;
                    case "По автору А-Я":
                        books = books.OrderBy(s => s.author);
                        break;
                    case "По автору Я-А":
                        books = books.OrderByDescending(s => s.author);
                        break;
                    case "По возрастанию цены":
                        books = books.OrderBy(s => s.price);
                        break;
                    case "По убыванию цены":
                        books = books.OrderByDescending(s => s.price);
                        break;
                    case "Арендовать книгу":
                        books = books.Where(s => s.status == "аренда");
                        break;
                    case "Купить книгу":
                        books = books.Where(s => s.status == "продажа");
                        break;
                    default:
                        books = books.OrderBy(s => s.name);
                        break;
                }

                List<string> sortList = new List<string>() { "По названию А-Я", "По названию Я-А", "По автору А-Я", "По автору Я-А", "По возрастанию цены", "По убыванию цены", "Арендовать книгу", "Купить книгу" };
                

                List<Category> categoriesList = _context.Category.ToList();
                // устанавливаем начальный элемент, который позволит выбрать всех
                categoriesList.Insert(0, new Category { name = "Все", id = 0 });

                //Поиск по названию и автору и фильтр по жанру
                if (category != null && Convert.ToInt32(category) != 0)
                {
                    books = books.Where(p => p.id_category == Convert.ToInt32(category));
                    ViewData["category"] = new SelectList(categoriesList, "id", "name", category);
                }else ViewData["category"] = new SelectList(categoriesList, "id", "name");

                if (!string.IsNullOrEmpty(name))
                {
                    books = books.Where(p => p.name!.Contains(name) || p.author!.Contains(name));
                    ViewData["name"] = name;
                }

                if (sort != null)
                {
                    ViewData["sort"] = new SelectList(sortList, sort);
                }
                else { ViewData["sort"] = new SelectList(sortList, "По названию А-Я"); }

                return View(await books.ToListAsync());

            }
            else
            {
                Problem("Entity set 'PoolOfBooksContext.Books'  is null.");
            }

            ViewData["category"] = new SelectList(_context.Category, "id", "name");

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
