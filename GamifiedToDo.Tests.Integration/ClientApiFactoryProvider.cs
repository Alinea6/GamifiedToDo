using GamifiedToDo.Tests.Integration.Host;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace GamifiedToDo.Tests.Integration;

static class ClientApiFactoryProvider
{
    public static WebApplicationFactory<Program> Create()
    {
        return Create((context, services) => {});
    }

    public static WebApplicationFactory<Program> Create(
        Action<WebHostBuilderContext, IServiceCollection> configureServices)
    {
        return new WebApplicationFactory<Program>()
            .WithWebHostBuilder(webHostBuilder => webHostBuilder
                .UseTestConfiguration()
                .ConfigureServices(configureServices));
    }
}