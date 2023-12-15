using Marketplace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Cryptography.X509Certificates;
using static System.Net.Mime.MediaTypeNames;

namespace Marketplace.Controllers;

[Authorize]
public class AuctionController : Controller
{
    private readonly MarketplaceContext _context;

    private readonly UserManager<User> _userManager;

    private readonly IWebHostEnvironment _webHostEnvironment;
    public AuctionController(MarketplaceContext context, UserManager<User> userManager, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;

    }
    private async Task<User> GetCurrentUserAsync()
    {
        return await _userManager.GetUserAsync(User);
    }
    public async Task<IActionResult> IndexAsync()
    {
        ViewData["CurrentUser"] = await GetCurrentUserAsync();
        return View(_context.Lots.Include(x => x.User).Include(x => x.Image).ToList());
    }

    [HttpGet("/auction/userLots/{id}")]
    public async Task<IActionResult> UserLots(int id)
    {
        ViewData["User"] = await GetCurrentUserAsync();
        return View(_context.Lots.Include(x => x.User).Include(x => x.Image).Where(x => x.User.Id == id).ToList());
    }

    [HttpGet("/auction/create")]
    public async Task<IActionResult> CreateAsync()
    {
        ViewData["User"] = await GetCurrentUserAsync();
        return View(new Lot());
    }

    [HttpPost("/auction/create")]
    public async Task<IActionResult> Create([FromForm] Lot lot, IFormFile? image)
    {
        if (image != null)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            if (lot.Image != null)
            {
                System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "itemImg", lot.Image.FileName));
            }
            using (var file = System.IO.File.OpenWrite(Path.Combine(_webHostEnvironment.WebRootPath, "itemImg", fileName)))
            {
                image.CopyTo(file);
            }
            lot.Image = new ImageFile { FileName = fileName };
        }
        lot.User = await GetCurrentUserAsync();
        lot.CreatedDate = DateTime.Now;
        _context.Lots.Add(lot);
        _context.SaveChanges();
        return Redirect($"/auction/userlots/{lot.User.Id}");
    }

    [HttpGet("/auction/infoAboutLot/{id}")]
    public async Task<IActionResult> InfoAboutLot(int id)
    {
        ViewData["AllBidsInLot"] = _context.Bids
            .Include(x => x.User)
            .Include(x => x.Lot)
            .Where(x => x.Lot.Id == id)
            .ToList()
            .OrderByDescending(x => x.Price).ToList();
        var bidsInLot = _context.Bids
       .Include(x => x.User)
       .Include(x => x.Lot)
       .Where(x => x.Lot.Id == id)
       .ToList();

        var higherBidPrice = bidsInLot.Any() ? bidsInLot.Max(x => x.Price) : 0;
        var currentLot = _context.Lots.Include(x => x.User).Include(x => x.Image).First(x => x.Id == id);
        var user = await GetCurrentUserAsync();
        ViewData["CurrentUser"] = user;
        if (_context.Bids
            .Include(x => x.User)
            .Include(x => x.Lot).Where(x => x.Lot.Id == id).ToList().Count() == 0)
        {
            return View(currentLot);
        }
        var higherBid = _context.Bids
            .Include(x => x.User)
            .Include(x => x.Lot)
            .First(x => x.Price == higherBidPrice);
        ViewData["UserPriceIsHighest"] = higherBid.User == user ? "true" : "false";
        ViewData["UserIdHighestBid"] = higherBid.User;
        ViewData["CurrentHigherPrice"] = higherBidPrice != null ?
            higherBidPrice.ToString() : currentLot.Price;
        return View(currentLot);
    }

    [HttpGet("/auction/edit/{id}")]

    public async Task<IActionResult> EditAsync(int id)
    {
        ViewData["User"] = await GetCurrentUserAsync();
        return View(_context.Lots.Include(x => x.User).Include(x => x.Image).First(x => x.Id == id));
    }

    [HttpPost("/auction/edit/{id}")]
    public async Task<IActionResult> EditAsync(int id, [FromForm] Lot lot, IFormFile? image)
     {
        var lotFromDb = _context.Lots
            .Include(x => x.User)
            .Include(x => x.Image)
            .First(x => x.Id == id);
        if (lotFromDb.IsExpired == true && lotFromDb.FinalDate != lot.FinalDate)
        {
            var currentHighestBid = _context.Bids
            .Include(x => x.Lot)
            .Where(x => x.Lot == lotFromDb)
            .ToList().Max(x => x.Price);
            lotFromDb.UserBuyer = null;
            lotFromDb.IsSold = false;
            lotFromDb.IsExpired = false;
            lotFromDb.User.Money -= currentHighestBid;
        }
        lotFromDb.FinalDate = lot.FinalDate;
        lotFromDb.Title = lot.Title;
        lotFromDb.Price = lot.Price;
        lotFromDb.Active = lot.Active;
        lotFromDb.IsSold = lot.IsSold;
        if (!lotFromDb.IsSold)
        {
            lotFromDb.MessageShown = false;
        }
        if (image != null)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            if (lotFromDb.Image != null)
            {
                System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "itemImg", lotFromDb.Image.FileName));
            }
            using (var file = System.IO.File.OpenWrite(Path.Combine(_webHostEnvironment.WebRootPath, "itemImg", fileName)))
            {
                image.CopyTo(file);
            }
            lotFromDb.Image = new ImageFile { FileName = fileName };
        }
        _context.SaveChanges();
        return Redirect($"/auction/userlots/{lotFromDb.User.Id}");
    }

    [HttpPost("/auction/bidCreate/{lotId}/{userIdHighestBid}")]

    public async Task<IActionResult> BidCreate(int lotId, int userIdHighestBid, [FromBody] Bid bid)
    {
        var user = await GetCurrentUserAsync();
        if (userIdHighestBid != 0)
        {
            var previousUser = _context.Users.First(x => x.Id == userIdHighestBid);
            var higherBidPrice = _context.Bids
                .Include(x => x.User)
                .Include(x => x.Lot)
                .Where(x => x.Lot.Id == lotId).ToList().Max(x => x.Price);
            previousUser.Money += higherBidPrice;
        }

        if (user.Money < bid.Price)
        {
            return BadRequest();
        }
        user.Money -= bid.Price;
        bid.TimeCreated = DateTime.Now;
        bid.User = user;
        bid.Lot = _context.Lots.First(x => x.Id == lotId);
        _context.Add(bid);
        _context.SaveChanges();
        return Ok();
    }

    [HttpPost("/auction/isExpired/{id}/{currentHighestPrice}")]
    public IActionResult IsExpired(int id, decimal currentHighestPrice)
    {
        var winnerLot = _context.Lots.Include(x => x.User).First(x => x.Id == id);
        var currentBid = _context.Bids
                .Include(x => x.User)
                .Include(x => x.Lot)
                .FirstOrDefault(x => x.Price == currentHighestPrice && x.Lot.Id == id);
        if (currentBid == null)
        {
            winnerLot.Active = false;
            _context.SaveChanges();
            return BadRequest("currentBid == null");
        }
        if (!winnerLot.IsExpired)
        {
            winnerLot.Active = false;
            winnerLot.IsSold = true;
            winnerLot.IsExpired = true;
            winnerLot.User.Money += currentHighestPrice;
            winnerLot.UserBuyer = currentBid.User;
            _context.SaveChanges();
            return Ok();
        }
        winnerLot.Active = false;
        winnerLot.IsSold = true;
        _context.SaveChanges();
        return BadRequest();
    }

    [HttpGet("/auction/purchases")]

    public async Task<IActionResult> Purchases()
    {
        var user = await GetCurrentUserAsync();
        ViewData["User"] = user;
        return View(_context.Lots
            .Include(x => x.User)
            .Include(x => x.UserBuyer)
            .Include(x => x.Image)
            .Where(x => x.UserBuyer == user && x.Active == false).ToList());
    }

    [HttpGet("/auction/soldOut")]
    public async Task<ActionResult> SoldOut()
    {
        var user = await GetCurrentUserAsync();
        ViewData["User"] = user;
        return View(_context.Lots
            .Include(x => x.User)
            .Include(x => x.UserBuyer)
            .Include(x => x.Image).Where(x => x.IsSold == true && x.User == user).ToList());
    }

    [HttpPost("/auction/userlots/delete/{id}")]
    public async Task<IActionResult> delete(int id)
    {
        _context.Remove(_context.Lots.First(x => x.Id == id));
        _context.SaveChanges();
        var user = await GetCurrentUserAsync();
        return Redirect($"/auction/userlots/{user.Id}");
    }

    [HttpGet("/auction/soldLots/{sellerId}")]

    public IActionResult SoldLots(int sellerId)
    {
        var soldItems = _context.Lots
            .Include(x => x.User)
            .Where(x => x.User.Id == sellerId && x.IsSold == true)
            .ToList();
        List<Lot> lotsToJson = new();
        foreach (var item in soldItems)
        {
            if (!item.MessageShown)
            {
                item.MessageShown = true;
                lotsToJson.Add(item);
            }
        }
        _context.SaveChanges();
        return Json(lotsToJson);
    }
}
