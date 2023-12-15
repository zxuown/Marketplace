using System.ComponentModel.DataAnnotations;

namespace Marketplace.Models;

public class Order
{
    public int Id { get; set; }

	public string Name { get; set; }

	public string SecondName { get; set; }

    public enum OrderStatus
    {
       New, Paid, Done
    }
    public OrderStatus Status { get; set; }

    public Item Item { get; set; }
    public string City { get; set; }

	public string Phone { get; set; }

	public string WareHouse { get; set; }

	public string Email { get; set; }
}
