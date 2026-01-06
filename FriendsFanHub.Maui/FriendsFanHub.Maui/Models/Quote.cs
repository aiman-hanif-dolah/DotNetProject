using System.ComponentModel.DataAnnotations;

namespace FriendsFanHub.Maui.Models;

public class Quote
{
    public int Id { get; set; }

    [Required, StringLength(1000)]
    public string Text { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Context { get; set; }

    [Required]
    public int CharacterId { get; set; }

    [Required]
    public int EpisodeId { get; set; }

    public Character? Character { get; set; }
    public Episode? Episode { get; set; }
}
