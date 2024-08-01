using landingpage.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using landingpage.Models;

namespace TempEmbeddin2302C2.Controllers
{
    public class ItemController : Controller
    {
        //_2302c2ecommerceContext db = new _2302c2ecommerceContext();

        private readonly ProductsdataContext db;

        public ItemController(ProductsdataContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            var data = db.Items.Include(item => item.Cat);
            return View(data.ToList());
        }

        public IActionResult Create()
        {
            ViewBag.CatId = new SelectList(db.Categories, "CatId", "CatName");
            return View();
        }
        [HttpPost]
        [HttpPost]
        public IActionResult Create(Item item, IFormFile file)
        {
            var imageName = DateTime.Now.ToString("yymmddhhmmss");

            imageName += Path.GetFileName(file.FileName);
            
            string imagepath = Path.Combine(HttpContext.Request.PathBase.Value, "wwwroot/Uploads");
            var imagevalue = Path.Combine(imagepath, imageName);
            using (var stream = new FileStream(imagevalue, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            string dbimage = Path.Combine("/Uploads", imageName);// /Uploads/8489327234846347apple.jpg
            item.Pimage = dbimage;

            db.Items.Add(item);
            db.SaveChanges();
            ViewBag.CatId = new SelectList(db.Categories, "CatId", "CatName");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item1 = db.Items.Include(i => i.Cat).SingleOrDefault(i => i.Id == id);
            if (item1 == null)
            {
                return NotFound(); // Handle not found cases
            }

            ViewBag.CatId = new SelectList(db.Categories, "CatId", "CatName");
            return View(item1);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Item item, IFormFile file, string oldimage)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CatId = new SelectList(db.Categories, "CatId", "CatName", item.CatId);
                return View(item);
            }

            var existingItem = db.Items.Include(i => i.Cat).SingleOrDefault(i => i.Id == item.Id);
            if (existingItem == null)
            {
                return NotFound(); 
            }

            existingItem.Name = item.Name;
            existingItem.Description = item.Description;
            existingItem.Price = item.Price;
            existingItem.CatId = item.CatId;

            if (file != null && file.Length > 0)
            {
                var imageName = DateTime.Now.ToString("yymmddhhmmss") + Path.GetFileName(file.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", imageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                existingItem.Pimage = Path.Combine("/Uploads", imageName);
            }
            else
            {
                existingItem.Pimage = oldimage;
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. The item was modified by another user.");
                return View(existingItem);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return View(existingItem);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var item1 = db.Items.Find(id);
            ViewBag.CatId = new SelectList(db.Categories, "CatId", "CatName");
            if (item1 != null)
            {

                return View(item1);
            }
            else
            {
                return RedirectToAction("Index");

            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Item item)
        {


            db.Items.Remove(item);
            db.SaveChanges();

            ViewBag.CatId = new SelectList(db.Categories, "CatId", "CatName");
            return RedirectToAction("Index");
        }




    }
}