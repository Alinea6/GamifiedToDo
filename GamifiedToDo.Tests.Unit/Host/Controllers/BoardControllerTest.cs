using System.Security.Claims;
using FluentAssertions;
using GamifiedToDo.API.Controllers;
using GamifiedToDo.API.Models.Boards;
using GamifiedToDo.Services.App.Int.Boards;
using GamifiedToDo.Tests.Unit.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Unit.Host.Controllers;

public class BoardControllerTest
{
    private const string UserId = "fake-user-id";
    private BoardController _sut;
    private Mock<IBoardService> _boardServiceMock;

    [SetUp]
    public void SetUp()
    {
        _boardServiceMock = new Mock<IBoardService>(MockBehavior.Strict);
        _sut = new BoardController(_boardServiceMock.Object);
        
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
        _boardServiceMock.VerifyAll();
    }

    [Test]
    public async Task GetById_should_call_board_service_and_return_board()
    {
        // arrange
        var expected = new Board();

        _boardServiceMock.Setup(x => x.GetById("fake-board-id", UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        // act
        var result = await _sut.GetById("fake-board-id");
        
        // assert
        result.Should().Be(expected);
    }

    [Test]
    public async Task Add_should_map_request_to_input__call_board_service_and_return_board()
    {
        // arrange
        var request = new BoardAddRequest
        {
            Name = "fake-board",
            ChoreIds = new List<string> { "fake-chore-id-1", "fake-chore-id-2" },
            Collaborators = new List<string> { "fake-user-1", "fake-user-2", "fake-user-3" }
        };
        var input = new BoardAddInput
        {
            UserId = UserId,
            Name = "fake-board",
            ChoreIds = new List<string> { "fake-chore-id-1", "fake-chore-id-2" },
            Collaborators = new List<string> { "fake-user-1", "fake-user-2", "fake-user-3" }
        };
        var expected = new Board();

        _boardServiceMock.Setup(x => x.Add(MoqHandler.IsEquivalentTo(input), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        // act
        var result = await _sut.Add(request);
        
        // assert
        result.Should().Be(expected);
    }

    [Test]
    public async Task DeleteById_should_call_board_service()
    {
        var id = "fake-board-id";

        _boardServiceMock.Setup(x => x.DeleteById(id, UserId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var act = () => _sut.DeleteById(id);

        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task GetUserBoards_should_call_board_service_and_return_list_of_boards()
    {
        var expected = new List<BoardListItem>();

        _boardServiceMock.Setup(x => x.GetUserBoards(UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.GetUserBoards();
        
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task AddChores_should_call_board_service_and_return_board()
    {
        var expected = new Board();
        var request = new BoardChoresRequest()
        {
            ChoreIds = new List<string> { "fake-chore-1", "fake-chore-2" }
        };
        var input = new BoardChoresInput
        {
            Id = "fake-board-id",
            UserId = UserId,
            ChoreIds = request.ChoreIds
        };

        _boardServiceMock.Setup(x => x.AddChores(MoqHandler.IsEquivalentTo(input), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.AddChores("fake-board-id", request);
        
        result.Should().Be(expected);
    }
    
    [Test]
    public async Task RemoveChores_should_call_board_service_and_return_board()
    {
        var expected = new Board();
        var request = new BoardChoresRequest()
        {
            ChoreIds = new List<string> { "fake-chore-1", "fake-chore-2" }
        };
        var input = new BoardChoresInput
        {
            Id = "fake-board-id",
            UserId = UserId,
            ChoreIds = request.ChoreIds
        };

        _boardServiceMock.Setup(x => x.RemoveChores(MoqHandler.IsEquivalentTo(input), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.RemoveChores("fake-board-id", request);
        
        result.Should().Be(expected);
    }

    [Test]
    public async Task AddCollaborators_should_call_board_service_and_return_board()
    {
        var expected = new Board();
        var request = new BoardCollaboratorsRequest
        {
            CollaboratorIds = new List<string> { "fake-collaborator-1", "fake-collaborator-2" }
        };
        var input = new BoardCollaboratorsInput
        {
            Id = "fake-board-id",
            UserId = UserId,
            CollaboratorIds = request.CollaboratorIds
        };

        _boardServiceMock
            .Setup(x => x.AddCollaborators(MoqHandler.IsEquivalentTo(input), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.AddCollaborators("fake-board-id", request);
        
        result.Should().Be(expected);
    }
    
    [Test]
    public async Task RemoveCollaborators_should_call_board_service_and_return_board()
    {
        var expected = new Board();
        var request = new BoardCollaboratorsRequest
        {
            CollaboratorIds = new List<string> { "fake-collaborator-1", "fake-collaborator-2" }
        };
        var input = new BoardCollaboratorsInput
        {
            Id = "fake-board-id",
            UserId = UserId,
            CollaboratorIds = request.CollaboratorIds
        };

        _boardServiceMock
            .Setup(x => x.RemoveCollaborators(MoqHandler.IsEquivalentTo(input), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.RemoveCollaborators("fake-board-id", request);
        
        result.Should().Be(expected);
    }
}