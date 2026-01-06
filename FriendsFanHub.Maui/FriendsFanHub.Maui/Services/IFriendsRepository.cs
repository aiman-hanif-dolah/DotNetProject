using FriendsFanHub.Maui.Models;
using LocationModel = FriendsFanHub.Maui.Models.Location;

namespace FriendsFanHub.Maui.Services;

public interface IFriendsRepository
{
    Task<List<Character>> GetCharactersAsync();
    Task<Character?> GetCharacterAsync(int id);
    Task<Character> AddCharacterAsync(Character character);
    Task UpdateCharacterAsync(Character character);
    Task DeleteCharacterAsync(int id);

    Task<List<Episode>> GetEpisodesAsync();
    Task<Episode?> GetEpisodeAsync(int id);
    Task<Episode> AddEpisodeAsync(Episode episode);
    Task UpdateEpisodeAsync(Episode episode);
    Task DeleteEpisodeAsync(int id);

    Task<List<LocationModel>> GetLocationsAsync();
    Task<LocationModel?> GetLocationAsync(int id);
    Task<LocationModel> AddLocationAsync(LocationModel location);
    Task UpdateLocationAsync(LocationModel location);
    Task DeleteLocationAsync(int id);

    Task<List<Quote>> GetQuotesAsync();
    Task<Quote?> GetQuoteAsync(int id);
    Task<Quote> AddQuoteAsync(Quote quote);
    Task UpdateQuoteAsync(Quote quote);
    Task DeleteQuoteAsync(int id);

    Task<DashboardSnapshot> GetDashboardAsync();
}
