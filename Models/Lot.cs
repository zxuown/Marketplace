namespace Marketplace.Models
{
    public class Lot
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime FinalDate { get; set;}

        public virtual ImageFile? Image { get; set; }

        public decimal Price {  get; set; }

        public User User { get; set; }

        public User? UserBuyer { get; set; }

        public bool Active { get; set; }

        public bool IsSold { get; set; }

        public bool MessageShown {  get; set; }

        public bool IsExpired { get; set; }
    }
}
