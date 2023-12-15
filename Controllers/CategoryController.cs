using Marketplace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Controllers
{
	[Authorize(Roles = "Admin")]
	public class CategoryController : Controller
	{
		private readonly MarketplaceContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public CategoryController(MarketplaceContext context, IWebHostEnvironment hostEnvironment)
		{
			_context = context;
			_webHostEnvironment = hostEnvironment;
		}
		public IActionResult Index()
		{
			return View(_context.Categories.Include(x => x.Image).ToList());
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View(new Category());
		}
		[HttpPost]
		public IActionResult Create([FromForm] Category category, IFormFile? image)
		{
			if (!ModelState.IsValid)
			{
				return View(category);
			}
			if (image != null)
			{
				var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
				if (category.Image != null)
				{
					System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "itemImg", category.Image.FileName));
				}
				using (var file = System.IO.File.OpenWrite(Path.Combine(_webHostEnvironment.WebRootPath, "itemImg", fileName)))
				{
					image.CopyTo(file);
				}
				category.Image = new ImageFile { FileName = fileName };
			}
			_context.Add(category);
			_context.SaveChanges();
			return Redirect("/Category/Index");
		}

		[HttpGet]
		public IActionResult Show(int id)
		{
			var category = _context.Categories.First(x => x.Id == id);
			return View(category);
		}
		[HttpGet("/Category/Edit/{id}")]
		public IActionResult Edit(int id)
		{
			var category = _context.Categories.Include(x => x.Image).First(x => x.Id == id);
			return View(category);
		}

		[HttpPost("/Category/Edit/{id}")]
		public IActionResult Edit(int id, [FromForm] Category categoryItem, IFormFile? image)
		{
			if (!ModelState.IsValid)
			{
				return View(categoryItem);
			}

			var category = _context.Categories.Include(x => x.Image).First(x => x.Id == id);
			if (image != null)
			{
				var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
				if (category.Image != null)
				{
					System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "itemImg", category.Image.FileName));
				}
				using (var file = System.IO.File.OpenWrite(Path.Combine(_webHostEnvironment.WebRootPath, "itemImg", fileName)))
				{
					image.CopyTo(file);
				}
                category.Image = new ImageFile { FileName = fileName };
			}

			category.Title = categoryItem.Title;
			category.Items = categoryItem.Items;
			_context.SaveChanges();
			return Redirect("/Category/Index");
		}

	}
}
