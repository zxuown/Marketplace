using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Marketplace.Models;

public class Item
{
	public Item()
	{
		Images = new HashSet<ImageFile>();
	}
	public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public string? Image { get; set; }
    [JsonIgnore]
    public string FullUrl => "/itemImg/" + Image;

	public virtual ICollection<ImageFile> Images { get; set; }

	public int CategoryId { get; set; }
	[ForeignKey(nameof(CategoryId))]
	public virtual Category Category { get; set; }

	public User User { get; set; }

    public User ? UserBuyer { get; set; }

    public bool Active { get; set; }

    public bool IsSold { get; set; } 

	public bool MessageShown { get; set; }
}
