namespace DotNetProject.Models;

public class DashboardViewModel
{
    public int CharacterCount { get; set; }
    public int EpisodeCount { get; set; }
    public int QuoteCount { get; set; }
    public int LocationCount { get; set; }
    public Quote? RandomQuote { get; set; }
    public List<Quote>? RecentQuotes { get; set; }
}
