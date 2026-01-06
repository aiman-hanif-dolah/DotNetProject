using System.ComponentModel.DataAnnotations;

namespace DotNetProject.Models;

public class Quote
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Quote text is required")]
    [StringLength(1000, ErrorMessage = "Quote cannot exceed 1000 characters")]
    [Display(Name = "Quote Text")]
    public string Text { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Context cannot exceed 500 characters")]
    public string? Context { get; set; }

    // Foreign key for Character
    [Required(ErrorMessage = "Please select a character")]
    [Display(Name = "Character")]
    public int CharacterId { get; set; }

    // Foreign key for Episode
    [Required(ErrorMessage = "Please select an episode")]
    [Display(Name = "Episode")]
    public int EpisodeId { get; set; }

    // Navigation properties
    public Character? Character { get; set; }
    public Episode? Episode { get; set; }
}
