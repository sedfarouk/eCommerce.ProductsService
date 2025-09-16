using System.Linq.Expressions;
using BusinessLogicLayer.DTO;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.ServiceContracts;

public interface IProductsService
{
    /// <summary>
    /// Retrieves the list of products from the products repository
    /// </summary>
    /// <returns>Returns list of all products as ProductResponse DTOs</returns>
    Task<List<ProductResponseDto>> GetProducts();

    /// <summary>
    /// Retrieves products that satisfy an expression
    /// </summary>
    /// <param name="condition">The condition to filter products</param>
    /// <returns>Returns a list of product response DTOs that satisfies condition</returns>
    Task<List<ProductResponseDto?>> GetProductsByCondition(Expression<Func<Product, bool>>? condition);
    
    /// <summary>
    /// Retrieves a product that satisfies an expression
    /// </summary>
    /// <param name="condition">The condition to filter for a product</param>
    /// <returns>Returns a product response DTOs that satisfies the specified condition</returns>
    Task<ProductResponseDto?> GetProductByCondition(Expression<Func<Product, bool>> condition);

    /// <summary>
    /// Adds/Inserts a product into the table using Products Repository
    /// </summary>
    /// <param name="addProductRequestDto">Product to insert</param>
    /// <returns>Product after inserting or null if unsuccessful</returns>
    Task<ProductResponseDto?> AddProduct(AddProductRequestDto addProductRequestDto);

    /// <summary>
    /// Updates the existing product based on the Product Id
    /// </summary>
    /// <param name="updateProductRequestDto">Product to update</param>
    /// <returns>Updated product after updating or null if unsuccessful</returns>
    Task<ProductResponseDto?> UpdateProduct(UpdateProductRequestDto updateProductRequestDto);

    /// <summary>
    /// Deletes an existing product based on the given product id
    /// </summary>
    /// <param name="productId">Product id of product to delete</param>
    /// <returns>True if deletion is successful, else false</returns>
    Task<bool> DeleteProduct(Guid productId);
}