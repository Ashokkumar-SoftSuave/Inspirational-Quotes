using System.ComponentModel.DataAnnotations;

namespace inspirational_quotes_Backend.Models
{
    public class Quote
    {
        [Key]
        public int Id { get; set; }
        public string Author { get; set; }
        public string Tags {  get; set; }
        public string QuoteDesp { get; set; }
    }
}
