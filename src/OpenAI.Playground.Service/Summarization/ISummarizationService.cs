using OpenAI.Playground.Service.Summarization.Models;

namespace OpenAI.Playground.Service.Summarization
{
    public interface ISummarizationService
    {
        Task<SummarizationResponse> TextSummarize(SummarizationRequest request);
    }
}
