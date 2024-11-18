namespace OpenAI.Playground.Service;

internal static class ReplacePromptPlaceholder
{
    public static string ReplacePlaceholders(
        this string prompt,
        Dictionary<string, string> placeholders
    )
    {
        foreach (var placeholder in placeholders)
        {
            prompt = prompt.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
        }
        return prompt;
    }
}
