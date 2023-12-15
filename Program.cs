using Marketplace.Models;
using Marketplace.Models.LiqPaySDK;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MarketplaceContext>(options =>
{
    options.UseSqlite("Data Source=MarketplaceContext.db");
    SQLitePCL.Batteries.Init();
});
builder.Services.AddMvc();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<LiqPay>();
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;

    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
})
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<MarketplaceContext>();
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}");
app.Run();
