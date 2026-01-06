using FriendsFanHub.Maui.Data;
using FriendsFanHub.Maui.Models;
using Microsoft.EntityFrameworkCore;
using LocationModel = FriendsFanHub.Maui.Models.Location;

namespace FriendsFanHub.Maui.Services;

public class FriendsRepository : IFriendsRepository
{
    private readonly AppDbContext _dbContext;

    public FriendsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Character>> GetCharactersAsync() =>
        _dbContext.Characters.AsNoTracking().OrderBy(c => c.Name).ToListAsync();

    public Task<List<Episode>> GetEpisodesAsync() =>
        _dbContext.Episodes.AsNoTracking().OrderBy(e => e.Season).ThenBy(e => e.EpisodeNumber).ToListAsync();

    public Task<List<LocationModel>> GetLocationsAsync() =>
        _dbContext.Locations.AsNoTracking().OrderBy(l => l.Name).ToListAsync();

    public Task<List<Quote>> GetQuotesAsync() =>
        _dbContext.Quotes
            .AsNoTracking()
            .Include(q => q.Character)
            .Include(q => q.Episode)
            .OrderByDescending(q => q.Id)
            .ToListAsync();

    public Task<Character?> GetCharacterAsync(int id) =>
        _dbContext.Characters.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Character> AddCharacterAsync(Character character)
    {
        _dbContext.Characters.Add(character);
        await _dbContext.SaveChangesAsync();
        return character;
    }

    public async Task UpdateCharacterAsync(Character character)
    {
        _dbContext.Characters.Update(character);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCharacterAsync(int id)
    {
        var entity = await _dbContext.Characters.FindAsync(id);
        if (entity is null)
        {
            return;
        }

        _dbContext.Characters.Remove(entity);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Cannot delete a character that still has quotes.", ex);
        }
    }

    public Task<Episode?> GetEpisodeAsync(int id) =>
        _dbContext.Episodes.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

    public async Task<Episode> AddEpisodeAsync(Episode episode)
    {
        _dbContext.Episodes.Add(episode);
        await _dbContext.SaveChangesAsync();
        return episode;
    }

    public async Task UpdateEpisodeAsync(Episode episode)
    {
        _dbContext.Episodes.Update(episode);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteEpisodeAsync(int id)
    {
        var entity = await _dbContext.Episodes.FindAsync(id);
        if (entity is null)
        {
            return;
        }

        _dbContext.Episodes.Remove(entity);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Cannot delete an episode that still has quotes.", ex);
        }
    }

    public Task<LocationModel?> GetLocationAsync(int id) =>
        _dbContext.Locations.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);

    public async Task<LocationModel> AddLocationAsync(LocationModel location)
    {
        _dbContext.Locations.Add(location);
        await _dbContext.SaveChangesAsync();
        return location;
    }

    public async Task UpdateLocationAsync(LocationModel location)
    {
        _dbContext.Locations.Update(location);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteLocationAsync(int id)
    {
        var entity = await _dbContext.Locations.FindAsync(id);
        if (entity is null)
        {
            return;
        }

        _dbContext.Locations.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public Task<Quote?> GetQuoteAsync(int id) =>
        _dbContext.Quotes
            .AsNoTracking()
            .Include(q => q.Character)
            .Include(q => q.Episode)
            .FirstOrDefaultAsync(q => q.Id == id);

    public async Task<Quote> AddQuoteAsync(Quote quote)
    {
        _dbContext.Quotes.Add(quote);
        await _dbContext.SaveChangesAsync();
        return quote;
    }

    public async Task UpdateQuoteAsync(Quote quote)
    {
        _dbContext.Quotes.Update(quote);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteQuoteAsync(int id)
    {
        var entity = await _dbContext.Quotes.FindAsync(id);
        if (entity is null)
        {
            return;
        }

        _dbContext.Quotes.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<DashboardSnapshot> GetDashboardAsync()
    {
        var quotes = await _dbContext.Quotes
            .AsNoTracking()
            .Include(q => q.Character)
            .Include(q => q.Episode)
            .OrderByDescending(q => q.Id)
            .ToListAsync();

        var snapshot = new DashboardSnapshot
        {
            CharacterCount = await _dbContext.Characters.CountAsync(),
            EpisodeCount = await _dbContext.Episodes.CountAsync(),
            QuoteCount = quotes.Count,
            LocationCount = await _dbContext.Locations.CountAsync(),
            RecentQuotes = quotes.Take(3).ToList()
        };

        if (quotes.Count > 0)
        {
            var random = new Random();
            snapshot.RandomQuote = quotes[random.Next(quotes.Count)];
        }

        return snapshot;
    }
}
