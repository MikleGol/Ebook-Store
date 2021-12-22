using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EbookSTR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ChartsController : ControllerBase
    {
        private readonly EbookContext _context;

        public ChartsController(EbookContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]

        public JsonResult JsonData()
        {
            var authors = _context.Authors.Include(b => b.Ebooks).ToList();

            List<object> catBook = new List<object>();
            catBook.Add(new[] { "Автор", "Кількість книжнок" });

            foreach (var c in authors)
            {
                catBook.Add(new object[] { c.Name, c.Ebooks.Count() });
            }
            return new JsonResult(catBook);
        }


        [HttpGet("JsonData2")]

        public JsonResult JsonData2()
        {
            var client = _context.Clients.Include(b => b.Purchases).ToList();

            List<object> catBook = new List<object>();
            catBook.Add(new[] { "Клієнт", "Кількість покупок" });

            foreach (var c in client)
            {
                catBook.Add(new object[] { c.Name, c.Purchases.Count() });
            }
            return new JsonResult(catBook);
        }

    }

}