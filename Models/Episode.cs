using System.ComponentModel.DataAnnotations;

namespace DotNetProject.Models;

public class Episode
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Episode title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Season number is required")]
    [Range(1, 10, ErrorMessage = "Season must be between 1 and 10")]
    public int Season { get; set; }

    [Required(ErrorMessage = "Episode number is required")]
    [Range(1, 30, ErrorMessage = "Episode number must be between 1 and 30")]
    [Display(Name = "Episode Number")]
    public int EpisodeNumber { get; set; }

    [Display(Name = "Air Date")]
    [DataType(DataType.Date)]
    public DateTime? AirDate { get; set; }

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; }

    [Display(Name = "Image URL")]
    [StringLength(500)]
    public string? ImageUrl { get; set; }

    [Display(Name = "Video URL")]
    [StringLength(500)]
    public string? VideoUrl { get; set; }

    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }

    [Display(Name = "Episode Code")]
    public string EpisodeCode => $"S{Season:D2}E{EpisodeNumber:D2}";

    // Navigation property
    public ICollection<Quote>? Quotes { get; set; }
}
