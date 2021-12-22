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
    public class EbooksController : Controller
    {
        private const string ERR_BOOK_EXISTS = "Така книга вже існує";
        private readonly EbookContext _context;
        
        public EbooksController(EbookContext context)
        {
            _context = context;
        }

        // GET: Ebooks
        public async Task<IActionResult> Index(int id, bool purchased)
        {
            List<Ebook> ebooks;
            ViewBag.AuthId = id;
            
            if (purchased)
            {
                ViewBag.Purchased = 1;
            }

            if (id == 0)
            {
                ebooks = await _context.Ebooks.Include(s => s.Author).ToListAsync();
            }
            else
            {
                ViewBag.Author = _context.Authors.Find(id).Name;
                ebooks = await _context.Ebooks.Where(s => s.AuthorId == id).Include(s => s.Author).ToListAsync();
            }

            return View(ebooks);
        }

        // GET: Ebooks/Create
        public IActionResult Create(int authId)
        {
            ViewBag.AuthId = authId;
            if (authId != 0)
            {
                ViewBag.Author = _context.Authors.Find(authId).Name;
            }
            ViewBag.AuthorList = authId == 0 ?
            new SelectList(_context.Authors, "Id", "Name") :
            new SelectList(new List<Author>() { _context.Authors.Find(authId) }, "Id", "Name");
            return View();
        }

        // POST: Ebooks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ebook ebook)
        {
            ViewBag.AuthId = ebook.AuthorId;
            ViewBag.Author = _context.Authors.Find(ebook.AuthorId).Name;

            bool duplicate = _context.Ebooks.Any(s => s.AuthorId == ebook.AuthorId && s.Name.Equals(ebook.Name));
            if (duplicate)
            {
                ModelState.AddModelError("Name", ERR_BOOK_EXISTS);
            }

            int k = _context.Ebooks.Count() + 2;
            while (_context.Ebooks.Any(s => s.Id == k))
            {
                k++;
            }
            ebook.Id = k;

            if(ebook.Price < 0)
            {
                ebook.Price *= -1;
            }

            if (ebook.Pages < 0)
            {
                ebook.Pages *= -1;
            }
            else if (ebook.Pages == 0)
            {
                ebook.Pages = 1;
            }

            if (ModelState.IsValid)
            {
                _context.Add(ebook);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = ebook.AuthorId });
            }

            ViewBag.AuthorList = new SelectList(_context.Authors, "Id", "Name", ebook.AuthorId);
            return View(ebook);
        }

        // GET: Ebooks/Edit/5
        public async Task<IActionResult> Edit(int id, int authId)
        {
            var ebook = await _context.Ebooks.FindAsync(id);
            ViewBag.AuthId = authId;
            ViewBag.AuthorList = new SelectList(_context.Authors, "Id", "Name", ebook.AuthorId);
            return View(ebook);
        }

        // POST: Ebooks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Ebook ebook, int authId)
        {
            ViewBag.AuthId = authId;
            bool duplicate = _context.Ebooks.Any(s => s.Id != ebook.Id && s.AuthorId == ebook.AuthorId && s.Name.Equals(ebook.Name));

            if (duplicate)
            {
                ModelState.AddModelError("Name", ERR_BOOK_EXISTS);
            }

            if (ebook.Price < 0)
            {
                ebook.Price *= -1;
            }

            if (ebook.Pages < 0)
            {
                ebook.Pages *= -1;
            }
            else if (ebook.Pages == 0)
            {
                ebook.Pages = 1;
            }

            if (ModelState.IsValid)
            {
                _context.Update(ebook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = authId });
            }

            ViewBag.AuthorList = new SelectList(_context.Authors, "Id", "Name", ebook.AuthorId);
            return View(ebook);
        }

        // POST: Ebooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ebook = await _context.Ebooks.FindAsync(id);
            try
            {
                _context.Ebooks.Remove(ebook);
                await _context.SaveChangesAsync();
            }catch(Exception ex) { }
            return RedirectToAction(nameof(Index));
        }
    }
}
