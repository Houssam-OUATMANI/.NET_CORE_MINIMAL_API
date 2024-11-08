using Api.Dtos;
using Microsoft.OpenApi.Models;

namespace Api.Endpoints;

public static class ProductEndpoint
{
    
    private static readonly List<ProductDto> Products = 
    [
        new (1, "title 1", "desc 1", 100, DateTime.Now, DateTime.Now),
        new (2, "title 2", "desc 2", 100, DateTime.Now, DateTime.Now),
    ];

    public static RouteGroupBuilder MapProductEndpoint(this WebApplication app)
    {
        var router = app.MapGroup("products").WithParameterValidation().WithOpenApi();   
        router.MapGet("/", () => Products).WithName("products.index");
        
        router.MapGet("/{id:int}", (int id) =>
        {
            var product = Products.Find(p => p.Id == id);
            return product is null
                ? Results.NotFound(new { message = $"Product with id {id} was not found" })
                : Results.Ok(product);
        }).WithName("products.show");
        
        router.MapPost("/", (CreateProductDto createProductDto) =>
        {
            var (name, description, price) = createProductDto;
            var np = new ProductDto(Products.Count + 1, name, description, price, DateTime.Now, DateTime.Now);
            Products.Add(np);
            return Results.CreatedAtRoute("products.show", new {id = np.Id}, np); 
        }).WithName("products.store");
        
        
        router.MapPut("/{id:int}", (int id, UpdateProductDto updateProductDto) =>
            {
                var (name, description, price) = updateProductDto;
                var index = Products.FindIndex(p => p.Id == id);
                if (index == -1) return Results.NotFound(new { message = $"Product with id {id} was not found" });
                
                Products[index] = Products[index] with
                {
                    Name = name, Description = description, Price = price, UpdatedAt = DateTime.Now
                };
                
                return Results.Ok(new {message = $"Product with id {id} was successfully updated" });
            }).WithName("products.update");
        
        
        router.MapDelete("/{id:int}", (int id) =>
        {
           return  Products.RemoveAll(p => p.Id == id) == 0 
               ? Results.NotFound(new { message = $"Product with id {id} was not found" })
               : Results.Ok(new {message = $"Product with id {id} was successfully deleted" });
                    
        }).WithName("products.destroy");
        return router;
    }
}