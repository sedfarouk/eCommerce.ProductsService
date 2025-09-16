using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities;

public class Product
{
    public Guid ProductId { get; init; }
    [MaxLength(50)]
    [Required]
    public string? ProductName { get; set; }
    [MaxLength(20)]
    public required string Category { get; set; }
    public int UnitPrice { get; set; }
    public int QuantityInStock { get; set; }
}