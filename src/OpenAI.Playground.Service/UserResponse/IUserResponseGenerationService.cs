using OpenAI.Playground.Service.UserResponse.Models;

namespace OpenAI.Playground.Service.UserResponse
{
    public interface IUserResponseGenerationService
    {
        Task<string> GenerateUserResponse(GenerateResponseRequest request);
    }
}
