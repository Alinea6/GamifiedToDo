using System.Net;
using System.Net.Http.Headers;
using System.Text;
using FluentAssertions;
using GamifiedToDo.API.Models;
using GamifiedToDo.API.Models.Users;
using GamifiedToDo.Services.App.Int.Users;
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
        var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiMTZmZGEwOTEtYjAzMS00M2YxLTk3OGItYzk3YjE3OWNjOGU2IiwiZXhwIjoxODI3MDU3MTQyLCJpc3MiOiJHYW1pZmllZFRvRG8uQVBJIiwiYXVkIjoiR2FtaWZpZWRUb0RvVXNlcnMifQ.Qr31Whi_f42oyicBicJGCfKXSX9WxS4bkHmKhkdDbjo";
    
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    
    [Test]
    public async Task UserController_should_register_and_login_user()
    {
        var userLogin = await RegisterShouldReturnUserId();
        await LoginShouldReturnToken(userLogin);
        await GetUsersShouldReturnListOfUsers();
    }

    private async Task<string> RegisterShouldReturnUserId()
    {
        var request = new RegisterRequest
        {
            Login = Guid.NewGuid().ToString(),
            Password = "password",
            Email = "email@example.com"
        };
        
        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("api/user/register", data);
        var responseString = await response.Content.ReadAsStringAsync();

        responseString.Should().NotBeEmpty();
        responseString.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return request.Login;
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
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task GetUsersShouldReturnListOfUsers()
    {
        var response = await _client.GetAsync("api/user");
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<User>>(responseString);

        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }
    
    [Test]
    public async Task CreateFriendRequest_should_not_throw_error()
    {
        var request = new FriendRequest()
        {
            FriendId = "1da994d7-2de9-4b30-8b32-d0f61ad4ff55"
        };
        
        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("api/user/friends/request", data);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}