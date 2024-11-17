using System.ComponentModel.DataAnnotations;

namespace Application.Models;

public class BookCreationRequest
{
    [Required]
    public string Title { get; set; } = "";
    [Required]
    public string Author { get; set; } = "";
    [Required]
    public bool IsCompleted { get; set; } = false;
}
