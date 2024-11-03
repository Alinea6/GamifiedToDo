using System.Net.Http.Headers;
using System.Text;
using FluentAssertions;
using GamifiedToDo.API.Models;
using GamifiedToDo.Services.App.Int;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Integration.Host.Controllers;

public partial class ChoreControllerTest
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
    public async Task GetUserChores_should_return_list_of_chores()
    {
        var response = await _client.GetAsync("api/chore");
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<Chore>>(responseString);

        result.Should().NotBeNull();
    }

    [Test]
    public async Task GetChoreById_should_return_chore()
    {
        var response = await _client.GetAsync("api/chore/99eeabcb-c420-47ed-93cf-d64d5c342b86");
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Chore>(responseString);

        result.Should().NotBeNull();
    }

    [Test]
    public async Task AddChore_should_add_chore_to_db_and_return_it()
    {
        var request = new ChoreUpdateRequest
        {
            ChoreText = "Clean house"
        };

        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("api/chore", data);
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Chore>(responseString);

        result.Should().NotBeNull();
    }
}