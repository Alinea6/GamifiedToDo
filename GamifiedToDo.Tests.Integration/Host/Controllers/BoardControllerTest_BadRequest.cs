using System.Net;
using System.Text;
using FluentAssertions;
using GamifiedToDo.API.Models.Boards;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Integration.Host.Controllers;

public partial class BoardControllerTest
{
    [TestCase(null)]
    [TestCase("")]
    public async Task Add_should_throw_exception_when_Name_is_empty_or_null(string? name)
    {
        var request = new BoardAddRequest
        {
            Name = name
        };

        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("api/board", data);


        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}