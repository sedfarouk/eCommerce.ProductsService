using System.Linq.Expressions;
using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository;

public class ProductsRepository(ApplicationDbContext dbContext) : IProductsRepository
{
    public async Task<Product?> AddProduct(Product product)
    {
        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();
        
        return product;
    }

    public async Task<bool> DeleteProduct(Guid productId)
    {
        Product? existingProduct = await dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

        if (existingProduct == null)
        {
            return false;
        }
        
        dbContext.Products.Remove(existingProduct);
        await dbContext.SaveChangesAsync();
        
        return true;
    }

    public async Task<Product?> UpdateProduct(Product newProduct)
    {
        Product? existingProduct = dbContext.Products.FirstOrDefault(p => p.ProductId == newProduct.ProductId);

        if (existingProduct == null)
        {
            return null;
        }
        
        existingProduct.ProductName = newProduct.ProductName;
        existingProduct.Category = newProduct.Category;
        existingProduct.UnitPrice = newProduct.UnitPrice;
        existingProduct.QuantityInStock = newProduct.QuantityInStock;

        dbContext.Products.Update(existingProduct);
        await dbContext.SaveChangesAsync();

        return newProduct;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        List<Product> allProducts = await dbContext.Products.ToListAsync();

        return allProducts;
    }

    public async Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        return await dbContext.Products.FirstOrDefaultAsync(conditionExpression);
    }

    public async Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        return await dbContext.Products.Where(conditionExpression).ToListAsync();
    }
}