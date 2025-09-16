using System.Text.Json.Serialization;
using AutoMapper;
using eCommerce.Products.API.DatabaseContext;
using eCommerce.Products.API.Mappers;
using eCommerce.Products.API.Middleware;
using eCommerce.Products.API.Models.DTO;
using eCommerce.Products.API.Models.Entities;
using eCommerce.Products.API.Models.Enums;
using eCommerce.Products.API.Repository;
using eCommerce.Products.API.RepositoryContracts;
using eCommerce.Products.API.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext to IoC container
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnectionString")));

builder.Services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Register repository services
builder.Services.AddTransient<IProductsRepository, ProductsRepository>();

// Add auto mapper to IoC
builder.Services.AddAutoMapper(cfg => {}, typeof(ProductMappingProfile).Assembly);

// Configure CORS
builder.Services.AddCors(opt => opt.AddDefaultPolicy(policyBuilder => policyBuilder.WithOrigins("https://localhost:4200")
    .AllowAnyMethod()
    .AllowAnyHeader()
));

// Add FluentValidations
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<AddProductRequestValidator>(); // Add AddProductRequestValidator

// Adding Endpoints ApiExplorer (Swagger to use to access endpoints)
builder.Services.AddEndpointsApiExplorer();

// Adding Swagger for api documentation
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Exception Handling
app.UseExceptionMiddlewareHandler();

// Routing
app.UseRouting();
app.UseSwagger(); // Enable swagger routing so we can access swagger json
app.UseSwaggerUI(); // Enable swagger UI

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

var productsGroup = app.MapGroup("/api/products");

productsGroup.MapGet("/", async (IProductsRepository productsRepository) => Results.Ok(await productsRepository.GetAllProducts()));

productsGroup.MapPost("/", async (AddProductRequestDto addProductRequestDto, IProductsRepository productsRepository) =>
{
    ProductResponseDto? productResponseDto = await productsRepository.AddProduct(addProductRequestDto);
        
    return Results.Ok(productResponseDto);
});

productsGroup.MapGet("/search/productid/{productId}",
    async (Guid productId, IProductsRepository productsRepository) =>
    {
        ProductResponseDto? searchResult = await productsRepository.GetProductById(productId);

        if (searchResult is null)
        {
            return Results.NotFound();
        }
        
        return Results.Ok(searchResult);
    });

productsGroup.MapGet("/search/{searchString}", async (string searchString, IProductsRepository productsRepository) =>
{
    List<ProductResponseDto> matchingProducts = await productsRepository.GetProductsByName(searchString);

    return Results.Ok(matchingProducts);
});

productsGroup.MapPut("/",
    async ([FromBody] UpdateProductRequestDto updateProductRequestDto, IProductsRepository productsRepository) =>
    {
        await productsRepository.UpdateProduct(updateProductRequestDto);

        return Results.NoContent();
    });

productsGroup.MapDelete("/{productId}", async (Guid productId, IProductsRepository productsRepository) =>
{
    bool isCompleted = await productsRepository.DeleteProduct(productId);

    return Results.Ok(new { isCompleted });
});

app.MapControllers();

app.Run();