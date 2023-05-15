
using System.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoolOfBooks.Data;
using PoolOfBooks.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace PoolOfBooks.Controllers
{
    public class UsersController : Controller
    {
        private readonly PoolOfBooksContext _context;
        private readonly INotyfService _toastNotification;
        private readonly ILogger<UsersController> _logger;


        public UsersController(PoolOfBooksContext context, INotyfService toastNotification, ILogger<UsersController> logger)
        {
            _context = context;
            _toastNotification = toastNotification;
            _logger = logger;
        }


        public IActionResult LoginNotify()
        {
            _toastNotification.Custom("Необходимо войти в свой аккаунт!", 6, "#602AC3", "fa fa-user");
            return RedirectToAction("Login");
        }
        public IActionResult OrderNotify()
        {
            var id = User.FindFirst("ID").Value;

            _toastNotification.Success("Заказ создан!\nМенеджер свяжется с Вами для подтверждения заказа", 10);

            return RedirectToAction("Details", new { id });
        }

        

        // GET: Users
        public async Task<IActionResult> Index()
        {
              return _context.Users != null ? 
                          View(await _context.Users.Include(x => x.Carts).Include(x => x.Order_Rent).Include(x => x.Order_Buy).ToListAsync()) :
                          Problem("Entity set 'PoolOfBooksContext.Users'  is null.");
        }

        // GET: Users/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users.Include(x => x.Order_Rent).Include(x => x.Order_Buy)
                .FirstOrDefaultAsync(m => m.id == id);
            ViewData["CartCount"] = _context.Cart.Where(x => x.userId == id).Count();


            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }


        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,login,password,role,phone,last_name,first_name,third_name,address")] Users users)
        {
            if (ModelState.IsValid)
            {
                if (users.login != null && users.password != null)
                {

                    try
                    {
                        
                        users.password = Crypto.Hash(users.password.ToString(), "SHA-256");

                        _context.Add(users);
                        await _context.SaveChangesAsync();
                        _toastNotification.Success("Вы успешно зарегистрированы!");
                        return Redirect($"~/Users/SignIn/{users.id}");
                    }
                    catch (Exception ex)
                    {
                        _toastNotification.Error("Ошибка регистрации!\n" + ex.Message);
                    }
                }
                else {
                    _toastNotification.Error("Логин и пароль обязательны для заполнения!");

                }
            }
            return View(users);
        }

        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("login,password")] Users model)
        {

            if (ModelState.IsValid)
            {
                if (model.login != null && model.password != null)
                {


                    var p = Crypto.Hash(model.password, "SHA-256");


                    var user = _context.Users.FirstOrDefaultAsync(u => u.login == model.login && u.password == p);

                    if (user.Result != null && user.Result.role != null)
                    {

                        int id = Convert.ToInt32(user.Result.id);

                        model.id = id;
                        _toastNotification.Success("Вы успешно вошли в аккаунт!");
                        return Redirect($"~/Users/SignIn/{id}");
                    }
                    else
                    {
                    _toastNotification.Error("Аккаунт не найден!");
                        return View(model);
                    }
                }
                else
                {
                    _toastNotification.Error("Введите логин и пароль!");
                }
            }
            else { 
                    _toastNotification.Error("Введены некорректные данные!");
            }
            return View();
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,login,password,role,phone,last_name,first_name,third_name,address")] Users users)
        {
            if (id != users.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect($"~/Users/Details/{users.id}");
            }
            return Redirect($"~/Users/Details/{users.id}");
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'PoolOfBooksContext.Users'  is null.");
            }
            var users = await _context.Users.FindAsync(id);
            if (users != null)
            {
                _context.Users.Remove(users);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
          return (_context.Users?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
