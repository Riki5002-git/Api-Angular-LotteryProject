using Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class DonorDTOs
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        //public List<string>? PresentsName { get; set; }
    }
}
