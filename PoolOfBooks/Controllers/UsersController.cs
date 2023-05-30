
using System.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoolOfBooks.Data;
using PoolOfBooks.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.AspNetCore.Hosting;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PoolOfBooks.Controllers
{
    public class UsersController : Controller
    {
        private readonly PoolOfBooksContext _context;
        private readonly INotyfService _toastNotification;
        private readonly ILogger<UsersController> _logger;
        IWebHostEnvironment _appEnvironment;

        public UsersController(PoolOfBooksContext context, INotyfService toastNotification, ILogger<UsersController> logger, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _toastNotification = toastNotification;
            _logger = logger;
            _appEnvironment = appEnvironment;
        }


        public IActionResult LoginNotify()
        {
            _toastNotification.Custom("Необходимо войти в свой аккаунт!", 6, "#602AC3", "fa fa-user");
            return RedirectToAction("Login");
        }

        public IActionResult ReportNotify()
        {
            _toastNotification.Custom("Отчет создан в папке \"Отчеты\"!", 6, "#602AC3", "fa fa-user");
            return RedirectToAction("Analize");
        }
        
        public IActionResult ReportRentNotify()
        {
            _toastNotification.Custom("Отчет создан в папке \"Отчеты\"!", 6, "#602AC3", "fa fa-user");
            return RedirectToAction("AnalizeRent");
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
                    var count = _context.Users.Where(u => u.login == users.login).Count();

                    if (count == 0)
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
                    else
                    {
                        _toastNotification.Error("Аккаунт с таким логином уже существует!");
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
            ViewData["role"] = new SelectList(new List<string> { "client", "admin" }, "client");
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

        // GET: Users/DitailsForAdmin/5
        public async Task<IActionResult> DetailsForAdmin(int? id)
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

        // GET: Users/Analize
        public IActionResult Analize()
        {
            _toastNotification.Warning("\nОтчет создан в папке Отчеты!\n", 10);

            var orders = _context.Order_Buy_Books.Include(x => x.Order_Buy).Include(x => x.Books).Where(x => x.Order_Buy.date.Month == DateTime.Today.Month && x.Order_Buy.date.Year == DateTime.Today.Year).ToList();

            if (orders == null)
            {
                return NotFound();
            }

                return View(orders);
        }

        

        // POST: Users/Analize
        [HttpPost, ActionName("Analize")]
        [ValidateAntiForgeryToken]
        public IActionResult Analize(DateTime start, DateTime end, string file) {


            Excel.Application winword = new Excel.Application()
            {
                //Отобразить Excel
                Visible = true,
                //Количество листов в рабочей книге
                SheetsInNewWorkbook = 1
            };
            Excel.Application app = new Excel.Application();
            
            //Добавить рабочую книгу
            Excel.Workbook workBook = app.Workbooks.Add(Type.Missing);

            //Отключить отображение окон с сообщениями
            app.DisplayAlerts = false;

            //Получаем первый лист документа (счет начинается с 1)
            Excel.Worksheet sheet = (Excel.Worksheet)app.Worksheets.get_Item(1);

            //Название листа (вкладки снизу).
            sheet.Name = string.Concat("Отчет ", start.ToString("dd.MM.yyyy"), " - ", end.ToString("dd.MM.yyyy"));

            var orders = _context.Order_Buy_Books.Include(x => x.Order_Buy).Include(x => x.Books).Where(x => x.Order_Buy.date.Date >= start.Date && x.Order_Buy.date.Date <= end.Date).ToList();

            
            sheet.Cells[1, 1] = string.Concat("Промежуток времени: ");
            sheet.Cells[1, 2] = string.Concat(start.ToString("dd.MM.yyyy"), " - ", end.ToString("dd.MM.yyyy"));

            //заполнение имен столбцов в excel
            
                sheet.Cells[3, 1] = "Дата";
                sheet.Cells[3, 2] = "id_книги";
                sheet.Cells[3, 3] = "Название";
                sheet.Cells[3, 4] = "Автор";
                sheet.Cells[3, 5] = "Сумма";

            decimal sum = 0;
            int j = 4;

            foreach (var order in orders)
            {

                sheet.Cells[j, 1] = order.Order_Buy.date.ToString("dd.MM.yyyy");
                sheet.Cells[j, 2] = order.id_book;
                sheet.Cells[j, 3] = order.Books.name.ToString();
                sheet.Cells[j, 4] = order.Books.author.ToString();
                sheet.Cells[j, 5] = order.Books.sellPrice.Value;
                j++;
                sum += order.Books.sellPrice.Value;
            }

            sheet.Cells[j, 4] = "Итог:";
            sheet.Cells[j, 5] = sum;
            sheet.Cells[j, 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft; 
            sheet.Cells[j, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight; 

            sheet.Columns.AutoFit();

            // и места где его нужно сохранить*/
            app.Application.ActiveWorkbook.SaveAs($"{_appEnvironment.WebRootPath}/Отчеты/{file}.xlsx", Type.Missing,
              Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
              Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            

            app.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);

            winword.Quit();


            return Redirect("~/Users/ReportNotify");
        }

        // GET: Users/Analize
        public IActionResult AnalizeRent()
        {

            var orders = _context.Order_Rent_Books.Include(x => x.Order_Rent).Include(x => x.Books).Where(x => x.Order_Rent.date_begin.Month == DateTime.Today.Month && x.Order_Rent.date_begin.Year == DateTime.Today.Year).ToList();

            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }



        // POST: Users/Analize
        [HttpPost, ActionName("AnalizeRent")]
        [ValidateAntiForgeryToken]
        public IActionResult AnalizeRent(DateTime start, DateTime end, string file)
        {

            Excel.Application winword = new Excel.Application()
            {
                //Отобразить Excel
                Visible = true,
                //Количество листов в рабочей книге
                SheetsInNewWorkbook = 1
            };
            Excel.Application app = new Excel.Application();

            //Добавить рабочую книгу
            Excel.Workbook workBook = app.Workbooks.Add(Type.Missing);

            //Отключить отображение окон с сообщениями
            app.DisplayAlerts = false;

            //Получаем первый лист документа (счет начинается с 1)
            Excel.Worksheet sheet = (Excel.Worksheet)app.Worksheets.get_Item(1);

            //Название листа (вкладки снизу).
            sheet.Name = string.Concat("Отчет ", start.ToString("dd.MM.yyyy"), " - ", end.ToString("dd.MM.yyyy"));

            var orders = _context.Order_Rent_Books.Include(x => x.Order_Rent).Include(x => x.Books).Where(x => x.Order_Rent.date_begin.Date >= start.Date && x.Order_Rent.date_begin.Date <= end.Date).ToList();


            sheet.Cells[1, 1] = string.Concat("Промежуток времени: ");
            sheet.Cells[1, 2] = string.Concat(start.ToString("dd.MM.yyyy"), " - ", end.ToString("dd.MM.yyyy"));

            //заполнение имен столбцов в excel

            sheet.Cells[3, 1] = "Дата начала";
            sheet.Cells[3, 2] = "Дата конца";
            sheet.Cells[3, 3] = "id_книги";
            sheet.Cells[3, 4] = "Название";
            sheet.Cells[3, 5] = "Автор";
            sheet.Cells[3, 6] = "Сумма";

            decimal sum = 0;
            int j = 4;

            foreach (var order in orders)
            {

                sheet.Cells[j, 1] = order.Order_Rent.date_begin.ToString("dd.MM.yyyy");
                sheet.Cells[j, 2] = order.Order_Rent.date_end.ToString("dd.MM.yyyy");
                sheet.Cells[j, 3] = order.id_book;
                sheet.Cells[j, 4] = order.Books.name.ToString();
                sheet.Cells[j, 5] = order.Books.author.ToString();
                sheet.Cells[j, 6] = order.Books.price.Value;
                j++;
                sum += order.Books.price.Value;
            }

            sheet.Cells[j, 5] = "Итог:";
            sheet.Cells[j, 6] = sum;
            sheet.Cells[j, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            sheet.Cells[j, 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

            sheet.Columns.AutoFit();

            // и места где его нужно сохранить*/
            app.Application.ActiveWorkbook.SaveAs($"{_appEnvironment.WebRootPath}/Отчеты/{file}.xlsx", Type.Missing,
              Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
              Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);



            app.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);

            winword.Quit();


            return Redirect("~/Users/ReportRentNotify");
        }
    }
}
