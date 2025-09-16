using eCommerce.Products.API.Models.DTO;
using eCommerce.Products.API.Models.Entities;

namespace eCommerce.Products.API.RepositoryContracts;

public interface IProductsRepository
{
    Task<ProductResponseDto?> AddProduct(AddProductRequestDto product);
    Task<bool> DeleteProduct(Guid productId);
    Task<ProductResponseDto?> GetProductById(Guid productId);
    Task<List<ProductResponseDto>> GetProductsByName(string productName);
    Task<ProductResponseDto?> UpdateProduct(UpdateProductRequestDto newProduct);
    Task<List<ProductResponseDto>> GetAllProducts();
}