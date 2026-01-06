using System.ComponentModel.DataAnnotations;

namespace DotNetProject.Models;

public class Character
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Character name is required")]
    [StringLength(100, ErrorMessage = "Character name cannot exceed 100 characters")]
    [Display(Name = "Character Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Actor name is required")]
    [StringLength(100, ErrorMessage = "Actor name cannot exceed 100 characters")]
    [Display(Name = "Actor Name")]
    public string ActorName { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    [StringLength(100, ErrorMessage = "Occupation cannot exceed 100 characters")]
    public string? Occupation { get; set; }

    [Display(Name = "Image URL")]
    [StringLength(500)]
    public string? ImageUrl { get; set; }

    [Display(Name = "Video URL")]
    [StringLength(500)]
    public string? VideoUrl { get; set; }

    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }

    // Navigation property
    public ICollection<Quote>? Quotes { get; set; }
}
