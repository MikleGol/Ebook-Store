using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EbookSTR.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly EbookContext _context;

        public PurchasesController(EbookContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int id)
        {
            List<Purchase> purchases;
            List<Ebook> ebooks;
            
            ViewBag.ClientId = id;

        

            if (id == 0)
            {
                purchases = await _context.Purchases.Include(p => p.Client).Include(p => p.Ebook).ToListAsync();
            }
            else
            {
                ViewBag.Client = _context.Clients.Find(id).Email;
                purchases = await _context.Purchases.Where(p => p.ClientId == id).Include(p => p.Ebook).ToListAsync();
            }

            foreach (var p in purchases)
            {
                ebooks = await _context.Ebooks.Where(s => s.Id == p.EbookId).Include(s => s.Author).ToListAsync();
                p.Ebook = ebooks[0];
            }

            return View(purchases);
        }

        public IActionResult Purchase(int ebookId, int authId)
        {
            FillViewBag(ebookId, authId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Purchase(Client model, int ebookId, int authId)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email.Equals(model.Email));
            bool duplicate = client == null ? false : _context.Purchases.Any(p => p.EbookId == ebookId && p.ClientId == client.Id);

            if (duplicate)
            {
                ModelState.AddModelError("Email", "Ви вже придбали цю книжку");
            }

            

            if (ModelState.IsValid)
            {                
                if (client == null)
                {
                    client = model;
                    int c = _context.Clients.Count() + 2;
                    while (_context.Clients.Any(s => s.Id == c))
                    {
                        c++;
                    }
                    client.Id = c;
                    await _context.Clients.AddAsync(client);
                    await _context.SaveChangesAsync();
                }

                int k = _context.Purchases.Count() + 2;
                while (_context.Purchases.Any(s => s.Id == k))
                {
                    k++;
                }
                
                var purchase = new Purchase() {Id = k , ClientId = client.Id, EbookId = ebookId, Date = DateTime.Now };
                await _context.Purchases.AddAsync(purchase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            FillViewBag(ebookId, authId);
            return View(model);
        }

        public void FillViewBag(int ebookId, int authId)
        {
            ViewBag.AuthId = authId;
            ViewBag.EbookId = ebookId;
            ViewBag.Ebooks = _context.Ebooks.Find(ebookId).Name;
        }
    }
}