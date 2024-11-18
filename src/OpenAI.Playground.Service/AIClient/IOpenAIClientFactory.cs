using OpenAI.Chat;

namespace OpenAI.Playground.Service.AIClient
{
    public interface IOpenAIClientFactory
    {
        ChatClient CreateChatClient(string modelName = "");
    }
}
