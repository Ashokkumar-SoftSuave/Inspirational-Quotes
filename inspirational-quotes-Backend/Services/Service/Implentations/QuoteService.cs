using inspirational_quotes_Backend.Models;
using inspirational_quotes_Backend.Services.Repositories.Abstractions;
using inspirational_quotes_Backend.Services.Service.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace inspirational_quotes_Backend.Services.Service.Implentations
{
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteRepository _quoteRepository;
        public QuoteService(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        public async Task<bool> Create(List<Quote> quote)
        {
            return await _quoteRepository.CreateAsync(quote);
        }

        public async Task<bool> Delete(int id)
        {
            var existingQuote = await _quoteRepository.GetAsync(id);
            if (existingQuote != null)
                return await _quoteRepository.DeleteAsync(existingQuote);
            return false;
        }

        public async Task<List<string>> GetAllTags()
        {
            return await _quoteRepository.GetAllTagsAsync();
        }

        public async Task<Quote> GetQuote(int id)
        {
            return await _quoteRepository.GetAsync(id);
        }

        public async Task<bool> Update(Quote quote)
        {
            var existingQuote = await _quoteRepository.GetAsync(quote.Id);
            if (existingQuote == null)
            {
                return false;
            }
            existingQuote.Author = quote.Author;
            existingQuote.Tags = quote.Tags;
            existingQuote.QuoteDesp = quote.QuoteDesp;
            return await _quoteRepository.UpdateAsync(existingQuote);
        }

        public async Task<List<Quote>> GetAll(SearchModel filter)
        {
            return await _quoteRepository.GetAllQuotes(filter);
        }

        public async Task<List<Quote>> GetAll()
        {
            return await _quoteRepository.GetAll();
        }
    }
}
