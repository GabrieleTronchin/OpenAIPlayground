using OpenAI.Playground.Service.SmartFactory.Models;

namespace OpenAI.Playground.Service.SmartFactory
{
    public interface IAIFactory
    {
        Task<FactoryResponse> Detect(FactoryRequest request);
    }
}
