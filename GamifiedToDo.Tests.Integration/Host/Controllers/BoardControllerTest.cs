using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Integration.Host.Controllers;

public class BoardControllerTest
{
    private WebApplicationFactory<Program> _clientApiFactory;
    private HttpClient _client;
    
    [OneTimeSetUp]
    public void SetUp()
    {
        _clientApiFactory = ClientApiFactoryProvider.Create((context, services) =>
        {
        });
        _client = _clientApiFactory.CreateClient();
        var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiMTZmZGEwOTEtYjAzMS00M2YxLTk3OGItYzk3YjE3OWNjOGU2IiwiZXhwIjoxODI3MDU3MTQyLCJpc3MiOiJHYW1pZmllZFRvRG8uQVBJIiwiYXVkIjoiR2FtaWZpZWRUb0RvVXNlcnMifQ.Qr31Whi_f42oyicBicJGCfKXSX9WxS4bkHmKhkdDbjo";
    
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    
    //TODO: Add integration test for GetById after Add endpoint is added
}