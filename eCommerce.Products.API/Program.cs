using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using BusinessLogicLayer;
using BusinessLogicLayer.DTO;
using DataAccessLayer;
using DataAccessLayer.RepositoryContracts;
using eCommerce.Products.API.ApiEndpoints;
using eCommerce.Products.API.Middleware;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add Data Access Layer and Business Logic Layer services
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer();

builder.Services.AddControllers();
    // .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); // Works for controllers

// For Minimal APIs - adding model binder to read values from JSON to enum
builder.Services.ConfigureHttpJsonOptions(opt => opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();

// Configure CORS
builder.Services.AddCors(opt => opt.AddDefaultPolicy(policyBuilder => policyBuilder.WithOrigins("http://localhost:4200")
    .AllowAnyMethod()
    .AllowAnyHeader()
));

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

// Add Minimal API endpoints
app.MapProductApiEndpoints();

app.MapControllers();

app.Run();