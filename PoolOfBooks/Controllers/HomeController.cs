using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoolOfBooks.Data;
using PoolOfBooks.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace PoolOfBooks.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly PoolOfBooksContext _context;


        public HomeController(ILogger<HomeController> logger, PoolOfBooksContext context)
		{
			_logger = logger;
			_context = context;
		}

	

		public async Task<IActionResult> IndexAsync()
		{

            return _context.Books != null ?
                         View(await _context.Books.Include(o => o.RentBooks).Include(o => o.Carts).Include(o => o.Category).ToListAsync()) :
                         Problem("Entity set 'PoolOfBooksContext.Books'  is null.");
        }

		public IActionResult Company()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}