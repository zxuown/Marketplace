namespace Marketplace.Models;

public class PayOrder
{
    public int Id { get; set; }

    public decimal Price { get; set; }

    public string Uid { get; set; }

    public User User { get; set; }

}
