namespace OpenAI.Playground.Service.SummarizeReviews.Models;

public class SummarizationReviewRequest
{
    public List<Review> Reviews { get; set; } = [];
    public List<string> OutputLanguages { get; set; } = ["en"];
    public int MaxNumberOfWord { get; set; } = 150;
}

public class Review
{
    public string Description { get; set; } = string.Empty;

    public int Score { get; set; } = 5;
}
