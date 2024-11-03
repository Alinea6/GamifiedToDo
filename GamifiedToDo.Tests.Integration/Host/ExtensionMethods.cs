using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace GamifiedToDo.Tests.Integration.Host;

public static class ExtensionMethods
{
    public static IWebHostBuilder UseTestConfiguration(this IWebHostBuilder builder)
    {
        var config = new ConfigurationBuilder()
            .AddTestConfiguration()
            .Build();

        return builder.UseConfiguration(config);
    }

    public static IConfigurationBuilder AddTestConfiguration(this IConfigurationBuilder builder)
    {
        return builder
            .AddJsonFile("appsettings.Test.json")
            .AddUserSecrets("89e19c44-9cc7-45f4-82ad-9bafbbc8ce52")
            .AddEnvironmentVariables();
    }
}