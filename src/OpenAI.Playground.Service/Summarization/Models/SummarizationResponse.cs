namespace OpenAI.Playground.Service.Summarization.Models;

public class SummarizationResponse
{
    public bool Success { get; set; } = false;

    public required dynamic Result { get; set; }
}
