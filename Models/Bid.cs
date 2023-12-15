namespace Marketplace.Models
{
	public class Bid
	{
		public int Id { get; set; }	

		public decimal Price { get; set; }

		public User User { get; set; }

		public DateTime TimeCreated { get; set; }	

		public Lot Lot { get; set; }
	}
}
