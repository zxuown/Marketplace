namespace Marketplace.Models;

public class Category
{
    public Category() 
    {
        Items = new HashSet<Item>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public virtual ImageFile? Image { get; set; }
    public virtual ICollection<Item> Items { get; set; }
}
