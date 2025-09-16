using eCommerce.Products.API.Models.Enums;

namespace eCommerce.Products.API.Models.Entities;

public class Product
{
    public Guid ProductId { get; set; }
    public string? ProductName { get; set; }
    public Category Category { get; set; }
    public int UnitPrice { get; set; }
    public int QuantityInStock { get; set; }
}