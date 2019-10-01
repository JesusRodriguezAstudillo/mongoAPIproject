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
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace mongoAPI.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IHostingEnvironment _env;        
        private readonly IItemRepository _itemDataAccess = new ItemRepository();
        
        public ItemsController(IHostingEnvironment hostingEnvironment) {
            _env = hostingEnvironment;
        }
        
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
                await _itemDataAccess.Add(i);
                return Json("New Item");
            }
            else
            {
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
            return View();
        }
    
        public string capitalFirstLetter(Match x)
        {
            string y = x.ToString().ToUpper();

            return y;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("_id,Name,Type,price")] Item item, IFormFile f)
        //public async Task<IActionResult> Create([Bind("_id,Name,Type,price")] Item item)
        {
            Regex r = new Regex(@"(^|\s)\S");
            item.Name = item.Name.ToLower();
            item.Type = item.Type.ToLower();
            
            item.Name = r.Replace(item.Name, new MatchEvaluator(capitalFirstLetter));
            item.Type = r.Replace(item.Type, new MatchEvaluator(capitalFirstLetter));

                Console.WriteLine("\n\n tes1\n\n");
            var x = await _itemDataAccess.GetItem(item.Name, true);

                Console.WriteLine("\n\n tes2\n\n");
            if(ModelState.IsValid && x == null)
            {
                await _itemDataAccess.Add(item);
                     
                if(f != null && f.Length > 0)
                {
                    string imgFileName = item.Name + ".png";
                    string imgFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", imgFileName);
                    
                    using(FileStream imagesFileStream = new FileStream(imgFilePath, FileMode.Create))
                    {
                        await f.CopyToAsync(imagesFileStream);
                    }
                }

                return Json( new { type = "newItem", nextAction = Url.Action("Index", "Items") } );
            }
            else if(x != null) {
                ModelState.AddModelError("Name", "The item already exists.");
                //TempData["present-item"] = x; // this line is giving me an error on the other side of the view
                Console.WriteLine("\n\n tes3\n\n");
                return Json( new { type = "presentItem", nextAction = x } );
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

        [HttpPost]
        public async Task<IActionResult> Edit(string id, [Bind("_id, Name, Type, price")] Item item, IFormFile imgFile)
        {
            Regex r = new Regex(@"(^|\s)\S");
            item.Name = item.Name.ToLower();
            item.Type = item.Type.ToLower();

            item.Name = r.Replace(item.Name, new MatchEvaluator(capitalFirstLetter));
            item.Type = r.Replace(item.Type, new MatchEvaluator(capitalFirstLetter));

            if(id != item._id)
            {
                return Json( new { data = "notFound" } ) ;
            }

            if(ModelState.IsValid)
            {
                await _itemDataAccess.Update(item);

                if(imgFile != null && imgFile.Length > 0)
                {
                    string imgFileName = item.Name + ".png";
                    string imgFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", imgFileName);
 
                    using(FileStream imageFileStream = new FileStream(imgFilePath, FileMode.Create))
                    {
                        await imgFile.CopyToAsync(imageFileStream);
                    }
                }

                //return RedirectToAction("Index");
                return Json(Url.Action("Index", "Items"));
            }

            //return View(item);
            return Json(new { data = "invalidItem" });;
        }
        public async Task<IActionResult> Delete(string id) 
        {
            if(id == null) 
            {
                return NotFound();
            }

            await _itemDataAccess.Delete(id);

            return RedirectToAction(nameof(Index));
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
        }*/
        /* 
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