using Marketplace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKit.NovaPoshta.Client;
using SKit.NovaPoshta.Client.Requests.Address;
using SKit.NovaPoshta.Client.Responses;
using SKit.NovaPoshta.Client.Services;
using SKit.NovaPoshta.Domain.Address;
namespace Marketplace.Controllers;

public class OrderController : Controller
{
    private readonly MarketplaceContext _context;
    private readonly UserManager<User> _userManager;
    public OrderController(MarketplaceContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    { 
        return View(_context.Orders.Include(x=>x.Item).ToList());
    }
    private async Task<User> GetCurrentUserAsync()
    {
        return await _userManager.GetUserAsync(User);
    }
    [HttpGet("/order/edit/{id}")]
    public IActionResult Edit(int id)
    {
        return View(_context.Orders.First(x => x.Id == id));
    }

    [HttpPost("/order/edit/{id}")]

    public IActionResult Edit(int id, [FromForm] Order order)
    {
        var dbOrder = _context.Orders.First(x => x.Id == id);
        dbOrder.Email = order.Email;
        dbOrder.City = order.City;
        dbOrder.SecondName = order.SecondName;
        dbOrder.Name = order.Name;
        dbOrder.Phone = order.Phone;
        dbOrder.WareHouse = order.WareHouse;
        dbOrder.Status = order.Status;
        _context.SaveChanges();
        return Redirect("/order/index");
    }

    [HttpGet("/order/create/{id}")]
    public async Task<IActionResult> Create(int id)
    {
        ViewData["currentUser"] = await GetCurrentUserAsync();
        ViewData["itemSold"] = _context.Items
            .Include(x => x.User)
            .Include(x => x.Category)
            .Include(x => x.Images)
            .First(x => x.Id == id);
        return View(new Order());
    }

    [HttpPost("/order/create/{id}")]
    public async Task<IActionResult> CreateAsync(int id, [FromBody] Order orderForm)
    {
        var item = _context.Items.First(x => x.Id == id);
        item.UserBuyer = await GetCurrentUserAsync();
        orderForm.Item = item;
        orderForm.Status = Order.OrderStatus.Paid;
        _context.Add(orderForm);
        _context.SaveChangesAsync();
        return Ok();
    }
    [HttpPost("/order/delete/{id}")]
    public IActionResult Delete(int id)
    {
        _context.Remove(_context.Orders.First(x => x.Id == id));
        _context.SaveChanges();
        return Redirect("/order/index");
    }

    [HttpPost("/order/novaPoshtaCity")]
    public async Task<IResponseEnvelope<NpCity>> NovaPoshtaCity([FromBody] WareHouseOrCity wareHouseOrCity)
    {
        var npGatewayOptions = new NovaPoshtaGatewayOptions(apiKey: "c6b5a49bffa88205108bd8f44961fc6e");
        var httpClient = new HttpClient();
        INovaPoshtaGateway npGateway = new NovaPoshtaGateway(httpClient, npGatewayOptions);
        INpAddressService npAddressService = new NpAddressService(npGateway);
        var requestProperties = new GetCitiesRequestProperties
        {
            FindByString = wareHouseOrCity.City,
            Page = 1,
            Limit = 1000
        };

        var citiesResponse = await npAddressService.GetCitiesAsync(requestProperties);

        return citiesResponse;
    }

    [HttpPost("/order/novaPoshtaWareHouse")]
    public async Task<IResponseEnvelope<NpWarehouse>> NovaPoshtaWareHouse([FromBody] WareHouseOrCity wareHouseOrCity)
    {
        var npGatewayOptions = new NovaPoshtaGatewayOptions(apiKey: "c6b5a49bffa88205108bd8f44961fc6e");
        var httpClient = new HttpClient();
        INovaPoshtaGateway npGateway = new NovaPoshtaGateway(httpClient, npGatewayOptions);
        INpAddressService npAddressService = new NpAddressService(npGateway);
        var prop = new GetWarehousesRequestProperties
        {
            CityName = wareHouseOrCity.City
        };
        var response = await npAddressService.GetWarehousesAsync(prop);
        return response;
    }
}
