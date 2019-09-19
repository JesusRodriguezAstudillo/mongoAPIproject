using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mongoAPI.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace mongoAPI.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IItemRepository _itemDataAccess = new ItemRepository();
        public async Task<ActionResult> Index()
        {
            IEnumerable<Item> items = await _itemDataAccess.GetItems();
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> ModalInsert([FromBody] Item i)
        {
            if(i == null)
            {
                return Json("Failed to add item.");
            }

            var x = await _itemDataAccess.GetItem(i.Name);
            if(x == null)
            {
                Console.WriteLine("the item not in database");
                await _itemDataAccess.Add(i);
                return Json("New Item");
            }
            else
            {
                Console.WriteLine("item in database");
                return Json("Item already in database");
            }

        }
        
        // GET: Items/Details/5
        public async Task<IActionResult> Details(string id)
        {
            Console.WriteLine("\n\n");
            Console.WriteLine(id);
            Console.WriteLine("\n\n");
            if(id == null)
            {
                return NotFound();
            }
            var i = await _itemDataAccess.GetItem(id);
            if (i == null)
            {
                return NotFound();
            }

            return View(i);
        }

        public IActionResult Create()
        {
            Item temp = new Item();
            temp._id = "";
            temp.Name = "";
            temp.Type = "";
            temp.price = 0;
            TempData["present-item"] = temp;
            
            return View();
        }
    
        public string capitalFirstLetter(Match x)
        {
            string y = x.ToString().ToUpper();

            return y;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("_id,Name,Type,price")] Item item, IFormFile f)
        {
            Regex r = new Regex(@"(^|\s)\S");
            item.Name = item.Name.ToLower();
            item.Type = item.Type.ToLower();
            
            item.Name = r.Replace(item.Name, new MatchEvaluator(capitalFirstLetter));
            item.Type = r.Replace(item.Type, new MatchEvaluator(capitalFirstLetter));

            Console.WriteLine(f.Name);

            var x = await _itemDataAccess.GetItem(item.Name, true);

            if(ModelState.IsValid && x == null)
            {
                await _itemDataAccess.Add(item);
                return RedirectToAction("Index");
            }
            else if(x != null) {
                ModelState.AddModelError("Name", "The item already exists.");
                TempData["present-item"] = x;
                return View(item);
            }
            else
            {
                return View(item);
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var item = await _itemDataAccess.GetItem(id);

            if(item == null)
            {
                return NotFound();
            }

            return View(item);
        }
    }
}

    /* 
    public class ItemsController : Controller
    {
        private readonly MongoApiDatabaseContext _context;

        public ItemsController(MongoApiDatabaseContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            return View(await _context.Item.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Type,price")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }*/

        /*/ GET: Items/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Name,Type,price")] Item item)
        {
            if (id != item.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ID))
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
            return View(item);
        }*/

/* 
        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var item = await _context.Item.FindAsync(id);
            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(string id)
        {
            return _context.Item.Any(e => e.ID == id);
        }
    }
    */