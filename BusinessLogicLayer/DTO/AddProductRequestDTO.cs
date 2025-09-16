using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.DTO;

public record AddProductRequestDto(string? ProductName, CategoryOptions Category, int UnitPrice, int QuantityInStock)
{
    public AddProductRequestDto() : this(default, default, default, default)
    {
        
    }
}