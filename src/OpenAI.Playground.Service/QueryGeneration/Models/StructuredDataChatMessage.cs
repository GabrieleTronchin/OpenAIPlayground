namespace OpenAI.Playground.Service.QueryGeneration.Models;

public class StructuredDataChatMessage
{
    public DateTime TimeStamp { get; set; }
    public string? Body { get; set; }
    public bool IsRequest { get; set; }

    public List<List<string>> RowData { get; set; } = [];
}
