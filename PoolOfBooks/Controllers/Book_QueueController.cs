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
    public class Book_QueueController : Controller
    {
        private readonly PoolOfBooksContext _context;

        public Book_QueueController(PoolOfBooksContext context)
        {
            _context = context;
        }

        // GET: Book_Queue
        public async Task<IActionResult> Index()
        {
              return _context.Book_Queue != null ? 
                          View(await _context.Book_Queue.ToListAsync()) :
                          Problem("Entity set 'PoolOfBooksContext.Book_Queue'  is null.");
        }

        // GET: Book_Queue/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Book_Queue == null)
            {
                return NotFound();
            }

            var book_Queue = await _context.Book_Queue
                .FirstOrDefaultAsync(m => m.id == id);
            if (book_Queue == null)
            {
                return NotFound();
            }

            return View(book_Queue);
        }

        // GET: Book_Queue/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Book_Queue/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,id_client,id_book")] Book_Queue book_Queue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book_Queue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book_Queue);
        }

        // GET: Book_Queue/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Book_Queue == null)
            {
                return NotFound();
            }

            var book_Queue = await _context.Book_Queue.FindAsync(id);
            if (book_Queue == null)
            {
                return NotFound();
            }
            return View(book_Queue);
        }

        // POST: Book_Queue/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,id_client,id_book")] Book_Queue book_Queue)
        {
            if (id != book_Queue.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book_Queue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Book_QueueExists(book_Queue.id))
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
            return View(book_Queue);
        }

        // GET: Book_Queue/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Book_Queue == null)
            {
                return NotFound();
            }

            var book_Queue = await _context.Book_Queue
                .FirstOrDefaultAsync(m => m.id == id);
            if (book_Queue == null)
            {
                return NotFound();
            }

            return View(book_Queue);
        }

        // POST: Book_Queue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Book_Queue == null)
            {
                return Problem("Entity set 'PoolOfBooksContext.Book_Queue'  is null.");
            }
            var book_Queue = await _context.Book_Queue.FindAsync(id);
            if (book_Queue != null)
            {
                _context.Book_Queue.Remove(book_Queue);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Book_QueueExists(int id)
        {
          return (_context.Book_Queue?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
