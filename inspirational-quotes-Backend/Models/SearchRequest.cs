namespace inspirational_quotes_Backend.Models
{
    public class SearchRequest
    {
        public string FilterName { get; set; }
        public string FilterValue { get; set; }
    }

    public class SearchModel
    {
        public string authorName { get; set; }
        public string tag { get; set; }
        public string desp { get; set; }
    }
}
