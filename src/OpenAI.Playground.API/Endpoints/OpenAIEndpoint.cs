using Microsoft.AspNetCore.Mvc;
using OpenAI.Playground.API.Endpoints.Primitives;
using OpenAI.Playground.Repository;
using OpenAI.Playground.Repository.FakeRepository;
using OpenAI.Playground.Repository.FakeRepository.PersonStore;
using OpenAI.Playground.Service.QueryGeneration;
using OpenAI.Playground.Service.SmartFactory;
using OpenAI.Playground.Service.SmartFactory.Models;
using OpenAI.Playground.Service.Summarization;
using OpenAI.Playground.Service.Summarization.Models;
using OpenAI.Playground.Service.SummarizeReviews;
using OpenAI.Playground.Service.SummarizeReviews.Models;
using OpenAI.Playground.Service.UserResponse;
using OpenAI.Playground.Service.UserResponse.Models;

namespace OpenAI.Playground.API.Endpoints;

public class OpenAIEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/Summarization",
                async (
                    [FromBody] SummarizationRequest request,
                    [FromServices] ISummarizationService service,
                    CancellationToken cancellationToken
                ) =>
                {
                    var result = await service.TextSummarize(request);

                    return result;
                }
            )
            .WithName("Summarization")
            .WithOpenApi();

        app.MapPost(
                "/SummarizationReviews",
                async (
                    [FromBody] SummarizationReviewRequest request,
                    [FromServices] ISummarizationReviewService service,
                    CancellationToken cancellationToken
                ) =>
                {
                    var result = await service.Summarize(request);

                    return result;
                }
            )
            .WithName("SummarizationReviews")
            .WithOpenApi();

        app.MapPost(
                "/AIFactory",
                async (
                    [FromBody] string question,
                    [FromServices] IAIFactory service,
                    [FromServices] IUserResponseGenerationService userResponseService,
                    [FromServices] IServiceProvider serviceProvider,
                    CancellationToken cancellationToken
                ) =>
                {
                    Dictionary<string, string> factoryInputs =
                        new()
                        {
                            {
                                typeof(FakeBookRepository).AssemblyQualifiedName!,
                                "when question is about books or any related topics"
                            },
                            {
                                typeof(FakePersonRepository).AssemblyQualifiedName!,
                                "when question is about person, people or any related topics"
                            },
                        };

                    var result = await service.Detect(
                        new FactoryRequest()
                        {
                            UserQuestion = question,
                            FactoryInputs = factoryInputs,
                        }
                    );

                    var type =
                        Type.GetType(result.Result)
                        ?? throw new InvalidOperationException(
                            "Unable to find data for this question."
                        );

                    IFakeRepository repository = (IFakeRepository)
                        serviceProvider.GetRequiredService(type);

                    var datas = repository.GetFakeData();

                    var response = await userResponseService.GenerateUserResponse(
                        new GenerateResponseRequest() { Datas = datas, UserQuestion = question }
                    );

                    return response;
                }
            )
            .WithName("AIFactory")
            .WithOpenApi();

        app.MapPost(
                "/SQLQuestion",
                async (
                    [FromBody] string question,
                    [FromServices] IQueryGenerationService service,
                    [FromServices] IUserResponseGenerationService userResponseService,
                    [FromServices] IDataRepository dataRepository
                ) =>
                {
                    var schema = await dataRepository.GetDatabaseSchema();

                    var query = await service.GenerateQuery(question, schema);

                    var dataResult = await dataRepository.ExecuteQuery(query);

                    var response = await userResponseService.GenerateUserResponse(
                        new GenerateResponseRequest()
                        {
                            Datas = dataResult,
                            UserQuestion = question,
                        }
                    );

                    return response;
                }
            )
            .WithName("SQLQuestion")
            .WithOpenApi();
    }
}
