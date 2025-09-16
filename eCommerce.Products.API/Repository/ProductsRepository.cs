using AutoMapper;
using AutoMapper.QueryableExtensions;
using eCommerce.Products.API.DatabaseContext;
using eCommerce.Products.API.Models.DTO;
using eCommerce.Products.API.Models.Entities;
using eCommerce.Products.API.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Products.API.Repository;

public class ProductsRepository : IProductsRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public ProductsRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<ProductResponseDto?> AddProduct(AddProductRequestDto addProductRequestDto)
    {
        Product product = _mapper.Map<Product>(addProductRequestDto);
        
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        
        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<bool> DeleteProduct(Guid productId)
    {
        Product? existingProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

        if (existingProduct == null)
        {
            return false;
        }
        
        _dbContext.Products.Remove(existingProduct);
        await _dbContext.SaveChangesAsync();
        
        return true;
    }

    public async Task<ProductResponseDto?> GetProductById(Guid productId)
    {
        Product? existingProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

        if (existingProduct == null)
        {
            return null;
        }

        return _mapper.Map<ProductResponseDto>(existingProduct);
    }

    public async Task<List<ProductResponseDto>> GetProductsByName(string productName)
    {
        List<ProductResponseDto> matchingProducts = await _dbContext.Products
            .Where(product => product.ProductName!.ToLower().Contains(
            productName.ToLower()))
            .ProjectTo<ProductResponseDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return matchingProducts;
    }

    public async Task<ProductResponseDto?> UpdateProduct(UpdateProductRequestDto newProduct)
    {
        Product? existingProduct = _dbContext.Products.FirstOrDefault(p => p.ProductId == newProduct.ProductId);

        if (existingProduct == null)
        {
            return null;
        }
        
        existingProduct.ProductName = newProduct.ProductName;
        existingProduct.Category = newProduct.Category;
        existingProduct.UnitPrice = newProduct.UnitPrice;
        existingProduct.QuantityInStock = newProduct.QuantityInStock;

        _dbContext.Products.Update(existingProduct);
        await _dbContext.SaveChangesAsync();
        
        return _mapper.Map<ProductResponseDto>(existingProduct);
    }

    public async Task<List<ProductResponseDto>> GetAllProducts()
    {
        List<Product> allProducts = await _dbContext.Products.ToListAsync();

        return allProducts.Select(product => _mapper.Map<ProductResponseDto>(product)).ToList();
    }
}