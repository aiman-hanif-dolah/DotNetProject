using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FriendsFanHub.Maui.Models;

public class Character
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string ActorName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? Occupation { get; set; }

    [StringLength(500)]
    public string? ImageUrl { get; set; }

    [StringLength(500)]
    public string? VideoUrl { get; set; }

    public ICollection<Quote>? Quotes { get; set; }
}
