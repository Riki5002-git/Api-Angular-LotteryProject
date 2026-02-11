using Api.Models;
using System.ComponentModel.DataAnnotations;

public class Purchase
{
    [Key]
    public int Id { get; set; }

    public int PresentId { get; set; }
    public Present? Present { get; set; }

    public int PersonId { get; set; }
    public Person? Person { get; set; }

    public DateTime PurchaseDate { get; set; }
}