using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Playground.Service.AIClient;
using OpenAI.Playground.Service.Options;
using OpenAI.Playground.Service.QueryGeneration;
using OpenAI.Playground.Service.SmartFactory;
using OpenAI.Playground.Service.Summarization;
using OpenAI.Playground.Service.SummarizeReviews;
using OpenAI.Playground.Service.UserResponse;

namespace OpenAI.Playground.Service;

public static class ServiceExtension
{
    public static IServiceCollection AddOpenAIServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<OpenAIOptions>(configuration.GetSection(OpenAIOptions.Name));
        services.AddTransient<ISummarizationService, SummarizationService>();

        services.AddTransient<IOpenAIClientFactory, OpenAIClientFactory>();
        services.AddTransient<IQueryGenerationService, QueryGenerationService>();
        services.AddTransient<ISummarizationReviewService, SummarizationReviewService>();
        services.AddTransient<IUserResponseGenerationService, UserResponseGenerationService>();

        services.AddTransient<IAIFactory, AIFactory>();

        return services;
    }
}
