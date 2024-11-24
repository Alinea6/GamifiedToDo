using System.Net.Http.Headers;
using System.Text;
using FluentAssertions;
using GamifiedToDo.API.Models.Chores;
using GamifiedToDo.Services.App.Int;
using GamifiedToDo.Services.App.Int.Chores;
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
        var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiMTZmZGEwOTEtYjAzMS00M2YxLTk3OGItYzk3YjE3OWNjOGU2IiwiZXhwIjoxODI3MDU3MTQyLCJpc3MiOiJHYW1pZmllZFRvRG8uQVBJIiwiYXVkIjoiR2FtaWZpZWRUb0RvVXNlcnMifQ.Qr31Whi_f42oyicBicJGCfKXSX9WxS4bkHmKhkdDbjo";
    
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    [Test]
    public async Task ChoreController_should_add_delete_and_get_chore()
    {
        var choreId = await AddShouldAddChoreToDbAndReturnIt();
        await GetUserChoresShouldReturnListOfChores();
        await GetByIdShouldReturnChore(choreId);
        await UpdateByIdShouldUpdateChore(choreId);
        await UpdateStatusByIdShouldUpdateChore(choreId);
        await DeleteByIdShouldRemoveChore(choreId);
    }

    private async Task GetUserChoresShouldReturnListOfChores()
    {
        var response = await _client.GetAsync("api/chore");
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<Chore>>(responseString);

        result.Should().NotBeNull();
    }
    
    private async Task GetByIdShouldReturnChore(string choreId)
    {
        var response = await _client.GetAsync($"api/chore/{choreId}");
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Chore>(responseString);

        result.Should().NotBeNull();
    }

    private async Task<string> AddShouldAddChoreToDbAndReturnIt()
    {
        var request = new ChoreAddRequest
        {
            ChoreText = "Clean house",
            Difficulty = ChoreDifficulty.Simple,
            Category = ChoreCategory.Cleaning
        };

        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("api/chore", data);
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Chore>(responseString);

        result.Should().NotBeNull();

        return result.Id;
    }

    private async Task DeleteByIdShouldRemoveChore(string choreId)
    {
        var act = () => _client.DeleteAsync($"api/chore/{choreId}");

        await act.Should().NotThrowAsync();
    }

    private async Task UpdateByIdShouldUpdateChore(string choreId)
    {
        var request = new ChoreUpdateRequest
        {
            ChoreText = "Clean bathroom",
            Difficulty = ChoreDifficulty.Moderate,
            Category = ChoreCategory.SelfCare
        };

        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"api/chore/{choreId}", data);
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Chore>(responseString);

        result.Should().NotBeNull();
    }
    
    private async Task UpdateStatusByIdShouldUpdateChore(string choreId)
    {
        var request = new ChoreUpdateStatusRequest()
        {
            Status = ChoreStatus.Done
        };

        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"api/chore/{choreId}/status", data);
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Chore>(responseString);

        result.Should().NotBeNull();
    }
}