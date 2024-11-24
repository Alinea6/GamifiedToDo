using System.Security.Claims;
using FluentAssertions;
using GamifiedToDo.API.Controllers;
using GamifiedToDo.API.Models.Chores;
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
    private string UserId = "fake-user-id";
    private ChoreController _sut;
    private Mock<IChoreService> _choreServiceMock;

    [SetUp]
    public void SetUp()
    {
        _choreServiceMock = new Mock<IChoreService>(MockBehavior.Strict);
        _sut = new ChoreController(_choreServiceMock.Object);
        
         var context = new DefaultHttpContext();

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
         };
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
                Difficulty = ChoreDifficulty.Simple,
                Category = ChoreCategory.Cleaning
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
    public async Task GetById_should_call_chore_service_and_return_chore()
    {
        // arrange
        var expected = new Chore();
        
        _choreServiceMock.Setup(x => x.GetChoreById("fake-chore-id", UserId,It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        // act
        var result = await _sut.GetById("fake-chore-id");
        
        // assert
        result.Should().Be(expected);
    }

    [Test]
    public async Task Add_should_call_chore_service_and_map_request_to_ChoreAddInput()
    {
        // arrange
        var expected = new Chore();
        var request = new ChoreAddRequest
        {
            ChoreText = "fake-chore-text",
            Difficulty = ChoreDifficulty.Simple,
            Category = ChoreCategory.Cleaning
        };
        var input = new ChoreAddInput
        {
            UserId = UserId,
            ChoreText = "fake-chore-text",
            Status = ChoreStatus.ToDo,
            Difficulty = ChoreDifficulty.Simple,
            Category = ChoreCategory.Cleaning
        };

        _choreServiceMock.Setup(x => x.AddChore(MoqHandler.IsEquivalentTo(input),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        // act
        var result = await _sut.Add(request);
        
        // assert
        result.Should().Be(expected);
    }

    [Test]
    public async Task DeleteById_should_call_chore_service()
    {
        var choreId = "fake-chore-id";

        _choreServiceMock.Setup(x => x.DeleteChoreById(choreId, UserId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var act = () => _sut.DeleteById(choreId);

        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task UpdateById_should_call_chore_service_and_return_chore()
    {
        // arrange
        var expected = new Chore();
        var request = new ChoreUpdateRequest()
        {
            ChoreText = "fake-chore-text",
            Difficulty = ChoreDifficulty.Simple,
            Category = ChoreCategory.Cleaning
        };
        var input = new ChoreUpdateInput()
        {
            Id = "fake-chore-id",
            UserId = UserId,
            ChoreText = "fake-chore-text",
            Difficulty = ChoreDifficulty.Simple,
            Category = ChoreCategory.Cleaning
        };

        _choreServiceMock.Setup(x => x.UpdateChoreById(MoqHandler.IsEquivalentTo(input),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        // act
        var result = await _sut.UpdateById("fake-chore-id", request);
        
        // assert
        result.Should().Be(expected);
    }
    
    [Test]
    public async Task UpdateStatusById_should_call_chore_service_and_return_chore()
    {
        // arrange
        var expected = new Chore();
        var request = new ChoreUpdateStatusRequest()
        {
            Status = ChoreStatus.Done
        };
        var input = new ChoreUpdateStatusInput()
        {
            Id = "fake-chore-id",
            UserId = UserId,
            Status = ChoreStatus.Done
        };

        _choreServiceMock.Setup(x => x.UpdateStatusById(MoqHandler.IsEquivalentTo(input),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        // act
        var result = await _sut.UpdateStatusById("fake-chore-id", request);
        
        // assert
        result.Should().Be(expected);
    }
}