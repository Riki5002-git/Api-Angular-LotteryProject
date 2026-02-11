using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Basket
    {
        [Key]
        public int Id { get; set; }
        public int PersonId { get; set; }
        public List<BasketItem>? Presents { get; set; }
    }

    public class BasketItem
    {
        [Key]
        public int Id { get; set; }
        public int PresentId { get; set; }
        public virtual Present? Present { get; set; }
        public int Quantity { get; set; }
    }
}