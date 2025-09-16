namespace BusinessLogicLayer.DTO;

public record ProductResponseDto(string  ProductId, string? ProductName, string? Category, int UnitPrice, int QuantityInStock);