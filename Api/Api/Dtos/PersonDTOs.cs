using Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class PersonDTOs
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "שם פרטי הוא שדה חובה")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "שם משפחה הוא שדה חובה")]
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "אימייל הוא שדה חובה")]
        [EmailAddress(ErrorMessage = "פורמט האימייל אינו תקין")]
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        //public List<Present>? MyPresents { get; set; }
        public int PurchasesAmount { get; set; } = 0;
    }
}
