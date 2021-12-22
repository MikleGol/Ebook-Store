using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EbookSTR;

namespace EbookSTR.Controllers
{
    public class AuthorsController : Controller
    {
        private const string ERR_AUTH_EXISTS = "Автор вже існує";
        private const string ERR_DELETE_AUTH_BOOKS = "Спочатку видаліть книжки автора";
        private readonly EbookContext _context;

        public AuthorsController(EbookContext context)
        {
            _context = context;
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Authors.Include("Genre").ToListAsync());
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var ebook = await _context.Ebooks.Where(s => s.AuthorId == id).Include("Comments").ToListAsync();
            ViewBag.Author = _context.Authors.Find(id).Name;
            return View();
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            ViewBag.GenreList = new SelectList(_context.Genres.ToList(), "Id", "Name");
            return View();
        }

        // POST: Authors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author)
        {
            bool duplicate = await _context.Authors.AnyAsync(d => d.Name.Equals(author.Name));

            if (duplicate)
            {
                ModelState.AddModelError("Name", ERR_AUTH_EXISTS);
            }
            int k = _context.Authors.Count() + 2;
            while (_context.Authors.Any(s => s.Id == k))
            {
                k++;
            }
            author.Id = k;
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.GenreList = new SelectList(_context.Genres.ToList(), "Id", "Name");
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var authors = await _context.Authors.FindAsync(id);
            ViewBag.GenreList = new SelectList(_context.Genres.ToList(), "Id", "Name");
            return View(authors);
        }

        // POST: Authors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Author author)
        {
            bool duplicate = await _context.Authors.AnyAsync(d => d.Name.Equals(author.Name) && d.GenreId == author.GenreId && d.Id != author.Id);

            if (duplicate)
            {
                ModelState.AddModelError("Name", ERR_AUTH_EXISTS);
            }

            if (ModelState.IsValid)
            {
                _context.Update(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.GenreList = new SelectList(_context.Genres.ToList(), "Id", "Name");
            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var authors = await _context.Authors.FindAsync(id);
            _context.Authors.Remove(authors);
            try
            {
                await _context.SaveChangesAsync();

            }catch(Exception ex) 
            {
                ModelState.AddModelError("Name", ERR_DELETE_AUTH_BOOKS);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
