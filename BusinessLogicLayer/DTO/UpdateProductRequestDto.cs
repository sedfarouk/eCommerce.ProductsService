using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.DTO;

public record UpdateProductRequestDto(
    Guid ProductId,
    string? ProductName,
    CategoryOptions Category,
    int UnitPrice,
    int QuantityInStock)
{
    public UpdateProductRequestDto() : this(default, default, default, default, default)
    {
        
    }
}