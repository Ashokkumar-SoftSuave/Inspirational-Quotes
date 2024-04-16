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
    public class QuoteRepository : IQuoteRepository
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

        public async Task<bool> DeleteAsync(Quote quote)
        {
            _db.Quotes.Remove(quote);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Quote?> GetAsync(int id)
        {
            return await _db.Quotes.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(Quote quote)
        {
            _db.Update(quote);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Quote>> GetFilteredQuotesAsync(string filterName, string filterValue)
        {
            IQueryable<Quote> filteredQuotes = _db.Quotes;
            if (filterValue != "" && filterValue != null)
            {
                switch (filterName.ToLower())
                {
                    case "author":
                        filteredQuotes = filteredQuotes.Where(q => q.Author.ToLower().Contains(filterValue));
                        break;
                    case "tag":
                        filteredQuotes = filteredQuotes.Where(q => q.Tags.ToLower().Contains(filterValue));
                        break;
                    case "desp":
                        filteredQuotes = filteredQuotes.Where(q => q.QuoteDesp.ToLower().Contains(filterValue));
                        break;
                    case "all":
                        filteredQuotes = filteredQuotes.Where(q =>
                            q.Author.ToLower().Contains(filterValue) ||
                            q.Tags.ToLower().Contains(filterValue) ||
                            q.QuoteDesp.ToLower().Contains(filterValue));
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

        public async Task<List<Quote>> GetAll()
        {
            return await _db.Quotes.ToListAsync();
        }

        public async Task<List<Quote>> GetAllQuotes(SearchModel filter)
        {
            try
            {
                var tags = (filter.tag ?? "").ToLower().Split(',').Select(t => t.Trim());
                var quotesQuery = _db.Quotes
                    .Where(_ =>
                        (_.Author.ToLower().Contains((filter.authorName ?? string.Empty).ToLower()) || string.IsNullOrEmpty(filter.authorName)) &&
                        (_.QuoteDesp.ToLower().Contains((filter.desp ?? string.Empty).ToLower()) || string.IsNullOrEmpty(filter.desp)));

                if (!string.IsNullOrEmpty(filter.tag))
                {
                    return quotesQuery.AsEnumerable().Where(q => tags.Any(_ => q.Tags.ToLower().Split(",", StringSplitOptions.None).Any(t => t.Equals(_)))).ToList();
                }
                return await quotesQuery.ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<List<string>> GetAllTagsAsync()
        {
            return await _db.Quotes.Select(t => t.Tags).ToListAsync();
        }

    }
}
