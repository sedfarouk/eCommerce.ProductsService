using eCommerce.Products.API.Models.Enums;

namespace eCommerce.Products.API.Models.DTO;

public record UpdateProductRequestDto(
    Guid ProductId,
    string? ProductName,
    Category Category,
    int UnitPrice,
    int QuantityInStock)
{
    public UpdateProductRequestDto() : this(default, default, default, default, default)
    {
        
    }
}