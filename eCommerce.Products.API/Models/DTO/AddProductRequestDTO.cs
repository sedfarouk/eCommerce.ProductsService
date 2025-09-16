using eCommerce.Products.API.Models.Enums;

namespace eCommerce.Products.API.Models.DTO;

public record AddProductRequestDto(string? ProductName, Category Category, int UnitPrice, int QuantityInStock)
{
    public AddProductRequestDto() : this(default, default, default, default)
    {
        
    }
}