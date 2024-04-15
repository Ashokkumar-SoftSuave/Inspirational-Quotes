using inspirational_quotes_Backend.Models;

namespace inspirational_quotes_Backend.Services.Service.Abstractions
{
    public interface IQuoteService
    {
        Task<bool> Create(List<Quote> quote);
        Task<bool> Update(Quote quote);
        Task<bool> Delete(int id);
        Task<Quote> GetQuote(int id);
        Task<List<string>> GetAllTags();
        Task<List<Quote>> GetAll(SearchModel filter);
        Task<List<Quote>> GetAll();
    }
}
