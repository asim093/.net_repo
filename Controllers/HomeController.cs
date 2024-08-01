using System.Diagnostics;

using landingpage.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace landingpage.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ProductsdataContext db;

        public HomeController(ProductsdataContext _db)
        {
            db = _db;
        }


        [Authorize(Roles = "User")]
        public IActionResult Index()
        {
            return View();
            //if (HttpContext.Session.GetString("role") == "user")
            //{
            //    return View();
            //}
            //else
            //{
            //    return RedirectToAction("login", "admin");
            //}
            }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult blog()
        {
            return View();
        }

        public IActionResult Products()
        {
            var data = db.Items.Include(item => item.Cat).ToList();
            return View(data);
        }



    }
}