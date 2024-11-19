using System.Net.Http.Headers;
using System.Text;
using FluentAssertions;
using GamifiedToDo.API.Models;
using GamifiedToDo.API.Models.Users;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Integration.Host.Controllers;

public partial class UserControllerTest
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
        var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiNzZlOWQ3NDktOTQ0Yy00OGM3LTg1NDUtOTAzMTNhZGU1ZTc4IiwiZXhwIjoxODAxMjU5NTYyLCJpc3MiOiJOb3Rlc1dpdGhUYWdzLkFQSSIsImF1ZCI6Ik5vdGVzV2l0aFRhZ3NVc2VycyJ9.2nGzD3k5p-FwIWzQQxaHnnG5nw7J1SkYOmd1aO5U50A";
    
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    
    [Test]
    public async Task UserController_should_register_and_login_user()
    {
        var userLogin = await RegisterShouldReturnUserId();
        await LoginShouldReturnToken(userLogin);
    }

    private async Task<string> RegisterShouldReturnUserId()
    {
        var request = new RegisterRequest
        {
            Login = Guid.NewGuid().ToString(),
            Password = "password"
        };
        
        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("api/user/register", data);
        var responseString = await response.Content.ReadAsStringAsync();

        responseString.Should().NotBeEmpty();
        responseString.Should().NotBeNull();
        return responseString;
    }

    private async Task LoginShouldReturnToken(string login)
    {
        var request = new LoginRequest
        {
            Login = login,
            Password = "password"
        };
        
        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("api/user/login", data);
        var responseString = await response.Content.ReadAsStringAsync();

        responseString.Should().NotBeEmpty();
        responseString.Should().NotBeNull();
    }
}