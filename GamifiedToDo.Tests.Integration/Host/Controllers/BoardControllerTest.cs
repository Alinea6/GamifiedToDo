using System.Net.Http.Headers;
using System.Text;
using FluentAssertions;
using GamifiedToDo.API.Models.Boards;
using GamifiedToDo.Services.App.Int.Boards;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Integration.Host.Controllers;

public partial class BoardControllerTest
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
    public async Task BoardController_should_add_delete_update_and_get_board()
    {
        var boardId = await AddShouldAddBoardToDbAndReturnIt();
        await GetUserBoardsShouldReturnBoardList();
        await GetByIdShouldReturnBoard(boardId);
        await AddChoresShouldAddChoreToBoard(boardId);
        await RemoveChoresShouldRemoveChoreFromBoard(boardId);
        await DeleteByIdShouldRemoveBoard(boardId);
    }

    private async Task<string> AddShouldAddBoardToDbAndReturnIt()
    {
        var request = new BoardAddRequest
        {
            Name = "test-board-name",
            Collaborators = new List<string>(),
            ChoreIds = new List<string>()
        };
        
        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("api/board", data);
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Board>(responseString);

        result.Should().NotBeNull();

        return result.Id;
    }

    private async Task GetByIdShouldReturnBoard(string boardId)
    {
        var response = await _client.GetAsync($"api/board/{boardId}");
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Board>(responseString);

        result.Should().NotBeNull();
    }

    private async Task DeleteByIdShouldRemoveBoard(string boardId)
    {
        var act = () => _client.DeleteAsync($"api/board/{boardId}");

        await act.Should().NotThrowAsync();
    }

    private async Task GetUserBoardsShouldReturnBoardList()
    {
        var response = await _client.GetAsync("api/board");
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<BoardListItem>>(responseString);

        result.Should().NotBeNull();
    }

    private async Task AddChoresShouldAddChoreToBoard(string boardId)
    {
        var request = new BoardChoresRequest()
        {
            ChoreIds = new List<string>{ "fake-id" }
        };
        
        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"api/board/{boardId}/chores/add", data);
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Board>(responseString);

        result.Chores.Should().NotBeEmpty();
    }

    private async Task RemoveChoresShouldRemoveChoreFromBoard(string boardId)
    {
        var request = new BoardChoresRequest()
        {
            ChoreIds = new List<string>{ "fake-id" }
        };
        
        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"api/board/{boardId}/chores/remove", data);
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Board>(responseString);

        result.Chores.Should().BeEmpty();
    }
}