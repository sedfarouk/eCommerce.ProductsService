using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using FluentValidation;
using FluentValidation.Results;

namespace eCommerce.Products.API.ApiEndpoints;

public static class ProductApiEndpoints
{
    public static IEndpointRouteBuilder MapProductApiEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var productsGroup = endpointRouteBuilder.MapGroup("/api/products"); 
        
        productsGroup.MapGet("/", async (IProductsService productsService) => Results.Ok(await productsService.GetProducts()));

        productsGroup.MapPost("/", async (AddProductRequestDto addProductRequestDto, IProductsService productsService, IValidator<AddProductRequestDto> addProductRequestValidator) =>
        {
            ValidationResult validationResult = await addProductRequestValidator.ValidateAsync(addProductRequestDto);

            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors.GroupBy(temp => temp.PropertyName).ToDictionary(grp => grp.Key, grp => grp.Select(err => err.ErrorMessage).ToArray());

                return Results.ValidationProblem(errors);
            }
            
            ProductResponseDto? productResponseDto = await productsService.AddProduct(addProductRequestDto);

            if (productResponseDto is null)
            {
                return Results.Problem("Error in adding product");
            }
        
            return Results.Created($"/api/products/search/product-id/{productResponseDto.ProductId}", productResponseDto);
        });

        productsGroup.MapGet("/search/productid/{productId:guid}",
            async (Guid productId, IProductsService productsService) =>
            {
                // await Task.Delay(100);
                // throw new NotImplementedException();
                
                ProductResponseDto? searchResult = await productsService.GetProductByCondition(p => p.ProductId == productId);

                if (searchResult is null)
                {
                    return Results.NotFound();
                }
        
                return Results.Ok(searchResult);
            });

        productsGroup.MapGet("/search/{searchString}", async (string searchString, IProductsService productsRepository) =>
        {
            List<ProductResponseDto?> matchingProductsByName = await productsRepository.GetProductsByCondition(p => p.ProductName != null && p.ProductName.ToLower().Contains(searchString.ToLower()));
            
            List<ProductResponseDto?> matchingProductsByCategory = await productsRepository.GetProductsByCondition(p => p.Category.ToLower().Contains(searchString.ToLower()));

            var matchingProducts = matchingProductsByName.Union(matchingProductsByCategory); 

            return Results.Ok(matchingProducts);
        });

        productsGroup.MapPut("/",
            async (UpdateProductRequestDto updateProductRequestDto, IProductsService productsService, IValidator<UpdateProductRequestDto> updateProductRequestValidator) =>
            {
                ValidationResult validationResult = await updateProductRequestValidator.ValidateAsync(updateProductRequestDto);

                if (!validationResult.IsValid)
                {
                    Dictionary<string, string[]> errors = validationResult.Errors.GroupBy(temp => temp.PropertyName).ToDictionary(grp => grp.Key, grp => grp.Select(err => err.ErrorMessage).ToArray());

                    return Results.ValidationProblem(errors);
                }
            
                ProductResponseDto? productResponseDto = await productsService.UpdateProduct(updateProductRequestDto);

                if (productResponseDto is null)
                {
                    return Results.Problem("Error in updating product");
                }
        
                return Results.Ok(productResponseDto);
            });

        productsGroup.MapDelete("/{productId:guid}", async (Guid productId, IProductsService productsService) =>
        {
            bool isDeleted = await productsService.DeleteProduct(productId);

            if (!isDeleted)
            {
                return Results.Problem("Error in deleting product");
            }

            return Results.Ok(true);
        });


        return endpointRouteBuilder;
    }
}