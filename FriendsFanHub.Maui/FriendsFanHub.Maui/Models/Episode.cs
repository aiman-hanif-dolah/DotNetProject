using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FriendsFanHub.Maui.Models;

public class Episode
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, Range(1, 10)]
    public int Season { get; set; }

    [Required, Range(1, 30)]
    public int EpisodeNumber { get; set; }

    [DataType(DataType.Date)]
    public DateTime? AirDate { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(500)]
    public string? ImageUrl { get; set; }

    [StringLength(500)]
    public string? VideoUrl { get; set; }

    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }

    public string EpisodeCode => $"S{Season:D2}E{EpisodeNumber:D2}";

    public ICollection<Quote>? Quotes { get; set; }
}
