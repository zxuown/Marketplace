using Marketplace.Models;
using Marketplace.Models.LiqPaySDK;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace Marketplace.Controllers;

public class HomeController : Controller
{
	private readonly MarketplaceContext _context;
    private readonly UserManager<User> _userManager;
    
    public HomeController(MarketplaceContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    private async Task<User> GetCurrentUserAsync()
    {
        return await _userManager.GetUserAsync(User);
    }
    public async Task<IActionResult> Index()
    {
		ViewData["AllItems"] = _context.Items.Include(x=>x.User).Include(x=>x.Category).ToList();
        ViewData["CurrentUser"] = await GetCurrentUserAsync();
        ViewData["Categories"] = _context.Categories.ToList();
        var categoryModelsAll = _context.Categories.Include(x => x.Items).First(x => x.Title == "All");
        categoryModelsAll.Items = _context.Items
        .Include(x => x.User)
        .Include(x => x.Category)
        .Include(x => x.Images).ToList();
        return View(categoryModelsAll);
    }

    
}
