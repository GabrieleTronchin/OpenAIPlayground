namespace OpenAI.Playground.Service.SummarizeReviews.Models;

public class SummarizationReviewResponse
{
    public bool Success { get; set; } = false;

    public required dynamic Result { get; set; }
}
