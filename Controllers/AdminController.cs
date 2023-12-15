using Marketplace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Controllers
{
    public class AdminController : Controller
    {
        private readonly MarketplaceContext _context;
        public AdminController(MarketplaceContext context) 
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewData["AllUsers"] = _context.Users.ToList();
            return View();
        }

        [HttpGet("/admin/edit/{id}")]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.First(x=>x.Id == id);
            return View(user);
        }

        [HttpPost("/admin/edit/{id}")]
        public IActionResult Edit(int id, [FromForm] User user) 
        {
            var UserFromDb = _context.Users.First(x => x.Id == id);
            UserFromDb.Money = user.Money;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
