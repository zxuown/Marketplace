using Marketplace.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Controllers
{
    public class PayOrderController : Controller
    {
        private readonly MarketplaceContext _context;

        private readonly UserManager<User> _userManager;
        public PayOrderController(MarketplaceContext context, UserManager<User> userManager) 
        { 
            _context = context;
            _userManager = userManager;
        }

        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new PayOrder());
        }
        [HttpPost]
        public async Task<IActionResult> Index([FromForm] PayOrder payOrder)
        {
            var user = await GetCurrentUserAsync();
            payOrder.Uid = Guid.NewGuid().ToString();
            payOrder.User = user;
            _context.Add(payOrder);
            await _context.SaveChangesAsync();
            return Redirect($"/item/liqpay/{payOrder.Uid}");
        }
    }
}
