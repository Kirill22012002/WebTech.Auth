using System.ComponentModel.DataAnnotations;

namespace WebTech.Auth.Models.FilterModels;

public class FilterDefinition
{
    [Required]
    public string Name { get; set; }

    [Required]
    [RegularExpression(@"^(eq|ne|lt|gt|le|ge|sw|ew|con)$", ErrorMessage = "Invalid operator.")]
    public string Operator { get; set; }

    [Required]
    public string Value { get; set; }
}