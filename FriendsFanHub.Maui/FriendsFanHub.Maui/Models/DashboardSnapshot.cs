using System.Collections.Generic;

namespace FriendsFanHub.Maui.Models;

public class DashboardSnapshot
{
    public int CharacterCount { get; set; }
    public int EpisodeCount { get; set; }
    public int QuoteCount { get; set; }
    public int LocationCount { get; set; }
    public Quote? RandomQuote { get; set; }
    public List<Quote> RecentQuotes { get; set; } = new();
}
