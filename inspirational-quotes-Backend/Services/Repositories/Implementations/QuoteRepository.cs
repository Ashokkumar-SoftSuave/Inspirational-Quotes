using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inspirational_quotes_Backend.DBContext;
using inspirational_quotes_Backend.Models;
using inspirational_quotes_Backend.Services.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace inspirational_quotes_Backend.Services.Repositories.Implementations
{
    public class QuoteRepository : IQuoteRepository, IDisposable
    {
        private readonly QuoteDBContext _db;

        public QuoteRepository(QuoteDBContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<bool> CreateAsync(List<Quote> quotes)
        {
            _db.Quotes.AddRange(quotes);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var quote = await _db.Quotes.FindAsync(id);
            if (quote != null)
            {
                _db.Quotes.Remove(quote);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Quote?> GetAsync(int id)
        {
            return await _db.Quotes.FindAsync(id);
        }       

        public async Task<bool> UpdateAsync(Quote quote)
        {
            var existingQuote = await GetAsync(quote.Id);
            if (existingQuote == null)
            {
                return false;
            }
            existingQuote.Author = quote.Author;
            existingQuote.Tags = quote.Tags;
            existingQuote.QuoteDesp = quote.QuoteDesp;
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<List<Quote>> GetFilteredQuotesAsync(string filterName, string filterValue)
        {
            IQueryable<Quote> filteredQuotes = _db.Quotes;
            if(filterValue != "" && filterValue != null)
            {
                switch (filterName.ToLower())
                {
                    case "author":
                        filteredQuotes = filteredQuotes.Where(q => q.Author.Contains(filterValue));
                        break;
                    case "tag":
                        filteredQuotes = filteredQuotes.Where(q => q.Tags.Contains(filterValue));
                        break;
                    case "desp":
                        filteredQuotes = filteredQuotes.Where(q => q.QuoteDesp.Contains(filterValue));
                        break;
                    case "all":
                        filteredQuotes = filteredQuotes.Where(q =>
                            q.Author.Contains(filterValue) ||
                            q.Tags.Contains(filterValue) ||
                            q.QuoteDesp.Contains(filterValue));
                        break;
                    default:
                        break;
                }
            }
            if (filteredQuotes != null)
            {
                var filterList = await filteredQuotes.Distinct().ToListAsync();
                return filterList;
            }
            return null!;
        }
        public async Task<List<string>> GetAllTagsAsync()
        {
            return await _db.Quotes.Select(t=> t.Tags).ToListAsync();
        }
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
