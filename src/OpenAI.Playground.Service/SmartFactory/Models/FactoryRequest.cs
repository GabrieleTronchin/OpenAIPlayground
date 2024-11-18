namespace OpenAI.Playground.Service.SmartFactory.Models;

public class FactoryRequest
{
    public required string UserQuestion { get; set; }

    public required Dictionary<string, string> FactoryInputs { get; set; }
}
