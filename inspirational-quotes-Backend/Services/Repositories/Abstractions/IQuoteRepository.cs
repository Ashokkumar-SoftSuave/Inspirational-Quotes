using inspirational_quotes_Backend.Models;

namespace inspirational_quotes_Backend.Services.Repositories.Abstractions
{
    public interface IQuoteRepository
    {
        Task<Quote?> GetAsync(int id);
        Task<bool> CreateAsync(List<Quote> quotes);
        Task<bool> UpdateAsync(Quote quote);
        Task<bool> DeleteAsync(int id);
        Task<List<Quote>> GetFilteredQuotesAsync(string filterName, string filterValue);
        Task<List<string>> GetAllTagsAsync();
    }
}
