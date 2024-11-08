using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;

public record CreateProductDto( 
    [Required] [Length(minimumLength: 2, maximumLength: 255)]
    string Name, 
    [Required]
    string Description,
    decimal Price);