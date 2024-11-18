namespace OpenAI.Playground.Service.Summarization.Models;

public class SummarizationRequest
{
    public required string TextToSummarize { get; set; }

    public List<string> TargetLanguages { get; set; } = ["en"];

    public int MaxNumberOfWord { get; set; } = 150;
}
