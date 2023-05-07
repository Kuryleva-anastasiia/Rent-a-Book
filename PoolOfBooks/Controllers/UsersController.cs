using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using PoolOfBooks.Data;
using PoolOfBooks.Models;

namespace PoolOfBooks.Controllers
{
    public class UsersController : Controller
    {
        private readonly PoolOfBooksContext _context;
        
        public UsersController(PoolOfBooksContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
              return _context.Users != null ? 
                          View(await _context.Users.ToListAsync()) :
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

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.id == id);
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
        public async Task<IActionResult> Create([Bind("id,login,password,role,last_name,first_name,third_name,address")] Users users)
        {
            if (ModelState.IsValid)
            {
                if (users.login != null && users.password != null)
                {
                    //Использование хранимой процедуры
                    var log = new Microsoft.Data.SqlClient.SqlParameter("@login", users.login);
                    var pass = new Microsoft.Data.SqlClient.SqlParameter("@password", users.password);
                    var role = new Microsoft.Data.SqlClient.SqlParameter("@role", "client");
                    var l_name = new Microsoft.Data.SqlClient.SqlParameter("@last_name", users.last_name);
                    var f_name = new Microsoft.Data.SqlClient.SqlParameter("@first_name", users.first_name);
                    var th_name = new Microsoft.Data.SqlClient.SqlParameter("@third_name", users.third_name);
                    var address = new Microsoft.Data.SqlClient.SqlParameter("@address", users.address);


                    try
                    {
                        
                        users.password = Crypto.Hash(users.password.ToString(), "SHA-256");

                        _context.Add(users);
                        await _context.SaveChangesAsync();
                        return Redirect($"~/Users/SignIn/{users.id}");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                }
                else { ModelState.AddModelError(string.Empty, "Введите логин и пароль!"); }
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

                        return Redirect($"~/Users/SignIn/{id}");
                    }
                    else
                    {
                        Results.NotFound(new { message = "Пользователь не найден" });
                        return View(model);
                    }
                }
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
        public async Task<IActionResult> Edit(int id, [Bind("id,login,password,role,last_name,first_name,third_name,address")] Users users)
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
                return RedirectToAction(nameof(Index));
            }
            return View(users);
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
