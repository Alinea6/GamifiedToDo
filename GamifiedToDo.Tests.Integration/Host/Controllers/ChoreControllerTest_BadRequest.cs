using System.Net;
using System.Text;
using FluentAssertions;
using GamifiedToDo.API.Models;
using GamifiedToDo.API.Models.Chores;
using GamifiedToDo.Services.App.Int;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Integration.Host.Controllers;

public partial class ChoreControllerTest
{
    [TestCase(null)]
    [TestCase("")]
    public async Task AddChore_should_throw_exception_when_ChoreText_is_empty_or_null(string? choreText)
    {
        var request = new ChoreAddRequest
        {
            ChoreText = choreText
        };

        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("api/chore", data);


        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}