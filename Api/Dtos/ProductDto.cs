namespace Api.Dtos;

public record ProductDto
(
    int Id,
    string Name,
    string Description,
    decimal Price,
    DateTime CreatedAt,
    DateTime UpdatedAt
);