using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetProject.Data;
using DotNetProject.Models;

namespace DotNetProject.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var quotes = await _context.Quotes
            .Include(q => q.Character)
            .Include(q => q.Episode)
            .ToListAsync();

        var randomQuote = quotes.Count > 0
            ? quotes[new Random().Next(quotes.Count)]
            : null;

        var viewModel = new DashboardViewModel
        {
            CharacterCount = await _context.Characters.CountAsync(),
            EpisodeCount = await _context.Episodes.CountAsync(),
            QuoteCount = await _context.Quotes.CountAsync(),
            LocationCount = await _context.Locations.CountAsync(),
            RandomQuote = randomQuote,
            RecentQuotes = quotes.OrderByDescending(q => q.Id).Take(3).ToList()
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}