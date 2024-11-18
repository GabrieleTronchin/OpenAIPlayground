namespace OpenAI.Playground.Service.Options;

public class OpenAIOptions
{
    public const string Name = "OpenAISettings";

    public string Key { get; set; } = string.Empty;

    public string Endpoint { get; set; } = string.Empty;

    public string DefaultModelName { get; set; } = string.Empty;
}
