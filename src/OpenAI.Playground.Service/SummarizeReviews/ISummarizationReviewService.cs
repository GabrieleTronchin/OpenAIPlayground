using OpenAI.Playground.Service.SummarizeReviews.Models;

namespace OpenAI.Playground.Service.SummarizeReviews;

public interface ISummarizationReviewService
{
    Task<SummarizationReviewResponse> Summarize(SummarizationReviewRequest request);
}
