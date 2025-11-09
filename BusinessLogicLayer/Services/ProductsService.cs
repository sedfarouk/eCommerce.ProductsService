using System.Linq.Expressions;
using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.RabbitMQ;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;

namespace BusinessLogicLayer.Services;

public class ProductsService(IProductsRepository productsRepository, IMapper mapper, IValidator<AddProductRequestDto> addProductRequestValidator, IValidator<UpdateProductRequestDto> updateProductRequestValidator, IRabbitMQPublisher rabbitMqPublisher) : IProductsService
{
    public async Task<List<ProductResponseDto>> GetProducts()
    {
        IEnumerable<Product> allProducts = (await productsRepository.GetProducts());

        IEnumerable<ProductResponseDto> productsResponse =
            mapper.Map<IEnumerable<ProductResponseDto>>(allProducts);

        return productsResponse.ToList();
    }

    public async Task<List<ProductResponseDto?>> GetProductsByCondition(Expression<Func<Product, bool>>? condition)
    {
        if (condition is null)
        {
            throw new ArgumentNullException(nameof(condition));
        }
        
        IEnumerable<Product?> matchingProducts = (await productsRepository.GetProductsByCondition(condition));

        IEnumerable<ProductResponseDto?> matchingProductsResponse =
            mapper.Map<IEnumerable<ProductResponseDto>>(matchingProducts);

        return matchingProductsResponse.ToList();
    }

    public async Task<ProductResponseDto?> GetProductByCondition(Expression<Func<Product, bool>> condition)
    {
        Product? matchingProduct = await productsRepository.GetProductByCondition(condition);

        if (matchingProduct is null)
        {
            return null;
        }

        return mapper.Map<ProductResponseDto>(matchingProduct);
    }

    public async Task<ProductResponseDto?> AddProduct(AddProductRequestDto addProductRequestDto)
    {
        if (addProductRequestDto == null)
        {
            throw new ArgumentNullException(nameof(addProductRequestDto));
        }
        
        // Validate the product using Fluent Validation
        ValidationResult validationResult = await addProductRequestValidator.ValidateAsync(addProductRequestDto);

        if (!validationResult.IsValid)
        {
            string errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage).ToList());

            throw new ArgumentException(errors);
        }

        Product product = mapper.Map<Product>(addProductRequestDto);

        Product? addedProduct = await productsRepository.AddProduct(product);

        if (addedProduct == null)
        {
            return null;
        }

        return mapper.Map<ProductResponseDto>(addedProduct);
    }

    public async Task<ProductResponseDto?> UpdateProduct(UpdateProductRequestDto updateProductRequestDto)
    {
        if (updateProductRequestDto == null)
        {
            throw new ArgumentNullException(nameof(updateProductRequestDto));
        }

        Product? existingProduct =
            await productsRepository.GetProductByCondition(product =>
                product.ProductId == updateProductRequestDto.ProductId);

        if (existingProduct is null)
        {
            throw new ArgumentException("Invalid Product Id");
        }

        ValidationResult validationResult = await updateProductRequestValidator.ValidateAsync(updateProductRequestDto);

        if (!validationResult.IsValid)
        {
            string errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage));

            throw new ArgumentException(errors);
        }
        
        // Check if product name changed
        bool isProductNameChanged = updateProductRequestDto.ProductName != existingProduct.ProductName;

        Product? updatedProduct = await productsRepository.UpdateProduct(existingProduct);
        
        if (updatedProduct is null)
        {
            return null;
        }
        
        if (isProductNameChanged)
        {
            string routingKey = "product.update.name";
            ProductNameUpdateMessage message = new ProductNameUpdateMessage(ProductId: updatedProduct!.ProductId, NewName: updatedProduct.ProductName);
                
            rabbitMqPublisher.Publish<ProductNameUpdateMessage>(routingKey: routingKey, message: message);
        }

        return mapper.Map<ProductResponseDto>(updatedProduct);
    }

    public async Task<bool> DeleteProduct(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(productId));
        }
        
        Product? existingProduct = await productsRepository.GetProductByCondition(temp => temp.ProductId == productId);

        if (existingProduct is null)
        {
            return false;
        }

        bool isDeleted = await productsRepository.DeleteProduct(productId);

        return isDeleted;
    }
}