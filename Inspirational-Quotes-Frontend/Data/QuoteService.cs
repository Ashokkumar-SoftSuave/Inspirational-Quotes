using Inspirational_Quotes_Frontend.ModelResponse;
using Inspirational_Quotes_Frontend.RequestModels;

namespace Inspirational_Quotes_Frontend.Data
{
    public class QuoteService
    {
        private readonly HttpClient _httpClient;

        public QuoteService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri("https://localhost:7097/");
        }

        public async Task<List<QuoteRequest>> GetQuotes()
        {
            var list = await _httpClient.GetFromJsonAsync<List<QuoteResponse>>("searchquote");
            var quoteList = list.Select(q => new QuoteRequest
            {
                Id = q.Id,
                Author = q.Author,
                Tags = q.Tags?.Split(",").ToList(),
                QuoteDesp = q.QuoteDesp
            }).ToList();
            return quoteList;
        } 
        public async Task<List<QuoteRequest>> GetQuotes(QuoteRequest quote)
        {
            var parameters = new List<KeyValuePair<string, string?>>()
            {
                new KeyValuePair<string, string?>("authorName", quote.Author ?? string.Empty),
                new KeyValuePair<string, string?>("tag", string.Join(", ",quote.Tags) ?? string.Empty),
                new KeyValuePair<string, string?>("desp", quote.QuoteDesp ?? string.Empty),
            };
            var list = await _httpClient.GetFromJsonAsync<List<QuoteResponse>>($"search{QueryString.Create(parameters).ToString()}");
            var quoteList = list.Select(q => new QuoteRequest
            {
                Id = q.Id,
                Author = q.Author,
                Tags = q.Tags?.Split(",").ToList(),
                QuoteDesp = q.QuoteDesp
            }).ToList();
            return quoteList;
        }
        public async Task<string> AddQuotes(List<QuoteRequest> quotes)
        {
            var quotesList = quotes.Select(q => new QuoteResponse {
                Author = q.Author,
                Tags = q.Tags.Count>0 ? string.Join(", ", q.Tags): string.Empty,
                QuoteDesp = q.QuoteDesp
            }).ToList();
            var response = await _httpClient.PostAsJsonAsync("createquotes", quotesList);

            if (response.IsSuccessStatusCode)
            {
                return "Quotes created successfully";
            }

            // Handle error response
            var errorMessage = await response.Content.ReadAsStringAsync();
            return $"Failed to create quotes: {errorMessage}";
        }
        public async Task<string> DeleteQuote(int id)
        {
            var response = await _httpClient.DeleteAsync($"deletequote/{id}");
            if (response.IsSuccessStatusCode)
            {
                return "Quote Deleted Successfully";
            }

            // Handle error response
            var errorMessage = await response.Content.ReadAsStringAsync();
            return $"Failed to Delete quote: {errorMessage}";
        }
        public async Task<QuoteRequest> GetQuoteById(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<QuoteResponse>($"getquote/{id}");
            var quote = new QuoteRequest { Id = response.Id, Author = response.Author, QuoteDesp = response.QuoteDesp, Tags = response.Tags?.Split(",").ToList() };
           return quote;
        }
        public async Task<string> UpadteQuote(QuoteRequest quoteRequest)
        {
            if (quoteRequest.Id != 0 && quoteRequest != null)
            {
                var quote = new QuoteResponse
                {
                    Id = quoteRequest.Id,
                    Author = quoteRequest.Author,
                    QuoteDesp = quoteRequest.QuoteDesp,
                    Tags = (quoteRequest.Tags.Count() > 1 ? string.Join(", ", quoteRequest.Tags) : quoteRequest.Tags?.ToString()) ?? string.Empty
                };

                var response = await _httpClient.PutAsJsonAsync($"updatequote/{quoteRequest.Id}", quote);
                if (response.IsSuccessStatusCode)
                {
                    return "Quote Updated Successfully";
                }
                var errorMessage = await response.Content.ReadAsStringAsync();
                return $"Failed to Update quote: {errorMessage}";
            }
            return $"Failed to Update quote: Quote Id is not available";
        }
    }
}
