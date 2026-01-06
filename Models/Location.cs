using System.ComponentModel.DataAnnotations;

namespace DotNetProject.Models;

public class Location
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Location name is required")]
    [StringLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Location type is required")]
    [StringLength(50, ErrorMessage = "Type cannot exceed 50 characters")]
    [Display(Name = "Location Type")]
    public string Type { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
    public string? Address { get; set; }

    [Display(Name = "Image URL")]
    [StringLength(500)]
    public string? ImageUrl { get; set; }

    [Display(Name = "Video URL")]
    [StringLength(500)]
    public string? VideoUrl { get; set; }

    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
}
