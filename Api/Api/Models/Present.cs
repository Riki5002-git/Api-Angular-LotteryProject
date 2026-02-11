using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Present
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DonorId { get; set; }
        [Required]
        public double Price { get; set; }
        public int? WinnerId { get; set; }
        public Person? Winner { get; set; }
        public string? PictureUrl { get; set; }
        public int CategoryId { get; set; }
        public int PurchasesAmount { get; set; }
    }
}
