using inspirational_quotes_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace inspirational_quotes_Backend.DBContext
{
    public class QuoteDBContext : DbContext
    {
        public QuoteDBContext(DbContextOptions<QuoteDBContext> options):base(options)
        {

        }
        public DbSet<Quote> Quotes { get; set; }
    }
}
