namespace OpenAI.Playground.Repository;

public partial class FakeBookRepository
{
    public class Book
    {
        public int id { get; set; }
        public string? title { get; set; }
        public string? author { get; set; }
        public string? genre { get; set; }
        public string? description { get; set; }
        public string? isbn { get; set; }
        public string? image { get; set; }
        public string? published { get; set; }
        public string? publisher { get; set; }
    }
}
