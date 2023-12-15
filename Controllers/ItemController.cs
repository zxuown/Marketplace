using Marketplace.Migrations;
using Marketplace.Models;
using Marketplace.Models.LiqPaySDK;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using User = Marketplace.Models.User;

namespace Marketplace.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly MarketplaceContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<User> _userManager;
        private readonly LiqPay _liqPay;
        public ItemController(MarketplaceContext context, IWebHostEnvironment hostEnvironment, UserManager<User> userManager, LiqPay liqPay)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
            _userManager = userManager;
            _liqPay = liqPay;
        }
        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }
        private async Task<List<Category>> CategoriesAll()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            ViewData["User"] = user;
            var temp = _context.Items
                .Include(x => x.User)
                .Include(x => x.Category)
                .Include(x => x.Images)
                .Where(x => x.User.Id == user.Id).ToList();
            return View(temp);
        }

        [HttpGet("/item/Purchases")]
        public async Task<IActionResult> Purchases()
        {
            var user = await GetCurrentUserAsync();
            ViewData["User"] = user;
            var userBuyers = _context.Items
                .Include(x => x.User)
                .Include(x => x.UserBuyer)
                .Include(x => x.Category)
                .Include(x => x.Images)
                .Where(x => x.UserBuyer == user)
                .ToList();
            return View(userBuyers);
        }

        [HttpGet("/item/SoldOut")]
        public async Task<IActionResult> SoldOut()
        {
            var user = await GetCurrentUserAsync();
            ViewData["User"] = user;
            return View(_context.Items
                .Include(x => x.User)
                .Where(x => x.User.Id == user.Id && x.IsSold == true)
                .ToList());
        }
        [HttpPost("/item/SetNewPriceUser")]
        public async Task<IActionResult> SetNewPriceUser([FromBody] ItemSold itemSold)
        {
            var user = await GetCurrentUserAsync();
            if (user.Money < itemSold.Price)
            {
                return BadRequest();
            }
            user.Money -= itemSold.Price;
            var userGetMoney = _context.Users.First(x => x.Id == itemSold.SellerId);
            userGetMoney.Money += itemSold.Price;
            var sold = _context.Items.Include(x => x.User).First(x => x.Id == itemSold.ItemSoldId);
            sold.Active = false;
            sold.IsSold = true;
            _context.SaveChanges();
            return Ok();
        }


        [HttpGet("/item/create")]
        public async Task<IActionResult> Create()
        {
            ViewData["Categories"] = await CategoriesAll();
            return View(new Item());
        }
        private async Task<ImageFile> Upload(IFormFile imageFile)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

            using (var file = System.IO.File.OpenWrite(Path.Combine(_webHostEnvironment.WebRootPath, "itemImg", fileName)))
            {
                await imageFile.CopyToAsync(file);
            }
            return new ImageFile { FileName = fileName };
        }
        private void RemoveImage(ImageFile imageFile)
        {
            System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "itemImg", imageFile.FileName));
            _context.Remove(imageFile);
        }

        [HttpPost("/item/create")]
        public async Task<IActionResult> Create([FromForm] Item item, IFormFile? image, ICollection<IFormFile>? images)
        {
            var user = await GetCurrentUserAsync();
            item.Category = await _context.Categories.FirstAsync(x => x.Id == item.CategoryId);
            if (image != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                if (item.Image != null)
                {
                    System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "itemImg", item.Image));
                }
                using (var file = System.IO.File.OpenWrite(Path.Combine(_webHostEnvironment.WebRootPath, "itemImg", fileName)))
                {
                    image.CopyTo(file);
                }
                item.Image = fileName;
            }
            if (images != null)
            {
                foreach (var img in images)
                {
                    item.Images.Add(await Upload(img));
                }
            }
            item.User = user;
            _context.Add(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet("/item/InfoAboutItem/{id}")]

        public async Task<IActionResult> InfoAboutItem(int id)
        {
            ViewData["currentUser"] = await  GetCurrentUserAsync();
            var item = _context.Items
                .Include(x=>x.User)
                .Include(x=>x.Category)
                .Include(x=>x.Images)
                .First(x => x.Id == id);
            return View(item);
        }

        [HttpPost("/item/CategorySort/{categoryTitle}")]
        public IActionResult CategoryProducts([FromBody] Category category)
        {
           
            var sortedCategory = _context.Categories.Include(x => x.Items).FirstOrDefault(x => x.Title == category.Title);
            if (category.Title == "All")
            {
                sortedCategory.Items = _context.Items
               .Include(x => x.User)
               .Include(x => x.Category)
               .Include(x => x.Images)
               .ToList();
                return PartialView("~/Views/Home/_CategoryItems.cshtml", sortedCategory);
            }
            if (sortedCategory == null)
            {
                return NotFound();
            }

            sortedCategory.Items = _context.Items
                .Include(x => x.User)
                .Include(x => x.Category)
                .Include(x => x.Images)
                .Where(x => x.Category.Title == category.Title && x.Active && !x.IsSold)
                .ToList();

            return PartialView("~/Views/Home/_CategoryItems.cshtml", sortedCategory);
        }
        private async Task<Item> FindOneAsync(int id)
        {
            var user = await GetCurrentUserAsync();
            return await _context.Items
                .Include(x => x.User)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstAsync(x => x.Id == id && x.User.Id == user.Id);
        }

        [HttpGet("/item/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await GetCurrentUserAsync();
            ViewData["Categories"] = await CategoriesAll();

            var item = await FindOneAsync(id);
            return View(item);
        }

        [HttpPost("/item/edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] Item item, IFormFile? image, ICollection<IFormFile>? images)
        {
            var user = await GetCurrentUserAsync();
            var itemFromDb = await FindOneAsync(id);
            itemFromDb.Title = item.Title;
            itemFromDb.Description = item.Description;
            itemFromDb.Price = item.Price;
            itemFromDb.Active = item.Active;
            itemFromDb.IsSold = item.IsSold;
            if (!itemFromDb.IsSold)
            {
                itemFromDb.MessageShown = false;
            }
            itemFromDb.Category = _context.Categories.First(x=>x.Id == item.CategoryId);

            if (image != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                if (item.Image != null)
                {
                    System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "itemImg", item.Image));
                }
                using (var file = System.IO.File.OpenWrite(Path.Combine(_webHostEnvironment.WebRootPath, "itemImg", fileName)))
                {
                    image.CopyTo(file);
                }
                item.Image = fileName;
                itemFromDb.Image = item.Image;
            }
            if (images != null && images.Count > 0)
            {
                foreach (var img in itemFromDb.Images)
                {
                    RemoveImage(img);
                }
                foreach (var img in images)
                {
                    itemFromDb.Images.Add(await Upload(img));
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost("/item/delete/{id}")]
        public async Task <IActionResult> Delete(int id)
        {
            var item = await FindOneAsync(id);
            foreach (var img in item.Images)
            {
                RemoveImage(img);
            }
            _context.Items.Remove(item);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("/item/liqpay/{uid}")]
        public async Task<IActionResult> Pay(string uid)
        {
            var payOrder = _context.PayOrders.Include(x => x.User).First(x => x.Uid == uid);
            var p = _liqPay.PayParams(payOrder.Price, "Need money", payOrder.Uid);
            ViewData["data"] = _liqPay.GetData(p);
            ViewData["signature"] = _liqPay.GetSignature(p);
            return View("LiqPay");
        }

        [HttpPost("/paymentResult")]
        public async Task<IActionResult> PayResult([FromBody] Notify notify)
        {
            if (!_liqPay.ValidateData(notify.Data, notify.Signature))
            {
                return BadRequest(new
                {
                    success = false,
                });
            }
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(notify.Data));
            var response = JsonConvert.DeserializeObject<PayResponse>(json);
            if (response.Status == "success")
            {
                // var order = _context.Orders.First(x => x.Uid == response.OrderId);
                //order.Status = Order.OrderStatus.Paid;
                var user = await GetCurrentUserAsync();
                user.Money += response.Amount;
                _context.SaveChanges();
            }
            return Ok(new
            {
                success = true,
            });
        }

        [HttpGet("/item/soldItems/{sellerId}")]

        public IActionResult SoldItems(int sellerId)
        {
            var soldItems = _context.Items
                .Include(x => x.User)
                .Where(x => x.User.Id == sellerId && x.IsSold == true)
                .ToList();
            List<Item> itemsToJson = new();
            foreach (var item in soldItems)
            {
                if (!item.MessageShown)
                {
                    item.MessageShown = true;
                    itemsToJson.Add(item);
                }
            }
            _context.SaveChanges();
            return Json(itemsToJson);
        }

    }
}
