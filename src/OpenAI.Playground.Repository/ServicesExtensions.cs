using Microsoft.Extensions.DependencyInjection;
using OpenAI.Playground.Repository.FakeRepository.PersonStore;

namespace OpenAI.Playground.Repository;

public static class ServicesExtensions
{
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddTransient<IDataRepository, DataRepository>();
        services.AddTransient<FakeBookRepository>();
        services.AddTransient<FakePersonRepository>();

        return services;
    }
}
