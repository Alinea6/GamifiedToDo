using System.Security.Claims;
using FluentAssertions;
using GamifiedToDo.API.Controllers;
using GamifiedToDo.API.Models;
using GamifiedToDo.Services.App.Int;
using GamifiedToDo.Services.App.Int.Chores;
using GamifiedToDo.Tests.Unit.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Unit.Host.Controllers;

public class ChoreControllerTest
{
    private string UserId = "490987db-6081-42ba-abf6-f09b0bad90b3";
    private ChoreController _sut;
    private Mock<IChoreService> _choreServiceMock;

    [SetUp]
    public void SetUp()
    {
        _choreServiceMock = new Mock<IChoreService>(MockBehavior.Strict);
        _sut = new ChoreController(_choreServiceMock.Object);
        
        
        //TODO: Uncomment after adding user controller and fix user ids in tests
        /*var context = new DefaultHttpContext();

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, "fake-user-id"),
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        context.User = claimsPrincipal;

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };*/
    }

    [TearDown]
    public void TearDown()
    {
        _choreServiceMock.VerifyAll();
    }

    [Test]
    public async Task GetUserChores_should_call_chore_service_and_return_list_of_chores()
    {
        // arrange
        var expected = new List<Chore>
        {
            new()
            {
                Id = "fake-chore-id",
                ChoreText = "fake-chore-text",
            }
        };

        _choreServiceMock.Setup(x => x.GetUserChores(UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        // act
        var result = await _sut.GetUserChores();
        
        // assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetChoreById_should_call_chore_service_and_return_chore()
    {
        // arrange
        var expected = new Chore();
        
        _choreServiceMock.Setup(x => x.GetChoreById("fake-chore-id", UserId,It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        // act
        var result = await _sut.GetChoreById("fake-chore-id");
        
        // assert
        result.Should().Be(expected);
    }

    [Test]
    public async Task AddChore_should_call_chore_service_and_map_request_to_ChoreAddInput()
    {
        // arrange
        var expected = new Chore();
        var request = new ChoreUpdateRequest
        {
            ChoreText = "fake-chore-text"
        };
        var input = new ChoreAddInput
        {
            UserId = UserId,
            ChoreText = "fake-chore-text",
            Status = ChoreStatus.ToDo
        };

        _choreServiceMock.Setup(x => x.AddChore(MoqHandler.IsEquivalentTo(input),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        // act
        var result = await _sut.AddChore(request);
        
        // assert
        result.Should().Be(expected);
    }

    [Test]
    public async Task DeleteChoreById_should_call_chore_service()
    {
        var choreId = "fake-chore-id";

        _choreServiceMock.Setup(x => x.DeleteChoreById(choreId, UserId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var act = () => _sut.DeleteChoreById(choreId);

        await act.Should().NotThrowAsync();
    }
}