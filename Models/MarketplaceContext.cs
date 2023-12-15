using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Models;

public class MarketplaceContext: IdentityDbContext<User, IdentityRole<int>, int>
{
	public MarketplaceContext(): base() { }
    public MarketplaceContext(DbContextOptions options) : base(options) { }
	public virtual DbSet<Item> Items { get; set; } = null!;

    public virtual DbSet<PayOrder> PayOrders { get; set; } = null!;

    public virtual DbSet<Order> Orders { get; set; } = null!;

    public override DbSet<User> Users { get; set; } = null!;

	public virtual DbSet<Category> Categories { get; set; } = null!;

    public virtual DbSet<Lot> Lots { get; set; } = null!;

	public virtual DbSet<Bid> Bids { get; set; } = null!;
	public virtual DbSet<ImageFile> ImageFiles { get; set; } = null!;
}
