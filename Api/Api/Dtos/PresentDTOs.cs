using Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class PresentDTOs
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "שם המתנה שדה חובה")]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "חובה לבחור תורם")]
        public int DonorId { get; set; }
        [Required(ErrorMessage = "מחיר הוא שדה חובה")]
        public double Price { get; set; }
        public int? WinnerId { get; set; }
        public Person? Winner { get; set; }
        public string? PictureUrl { get; set; }
        public int CategoryId { get; set; }
        public int PurchasesAmount { get; set; }
    }
}
