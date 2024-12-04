using FluentAssertions;
using GamifiedToDo.Services.App.Boards;
using GamifiedToDo.Services.App.Dep.Boards;
using GamifiedToDo.Services.App.Int.Boards;
using Moq;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Unit.Services;

public class BoardServiceTest
{
    private BoardService _sut;
    private Mock<IBoardRepository> _boardRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        _boardRepositoryMock = new Mock<IBoardRepository>(MockBehavior.Strict);
        _sut = new BoardService(_boardRepositoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _boardRepositoryMock.VerifyAll();
    }

    [Test]
    public async Task GetById_should_call_board_repository_and_return_board()
    {
        var boardId = "fake-board-id";
        var userId = "fake-user-id";
        var expected = new Board();
        
        _boardRepositoryMock.Setup(x => x.GetById(boardId, userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        // act
        var result = await _sut.GetById(boardId, userId);
        
        // assert
        result.Should().Be(expected);
    }

    [Test]
    public async Task Add_should_call_board_repository_and_return_board()
    {
        var input = new BoardAddInput();
        var expected = new Board();

        _boardRepositoryMock.Setup(x => x.Add(input, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.Add(input);
        
        result.Should().Be(expected);
    }

    [Test]
    public async Task Delete_should_call_board_repository()
    {
        var id = "fake-board-id";
        
        _boardRepositoryMock.Setup(x => x.DeleteById(
                id, 
                "fake-user-id", 
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var act = () => _sut.DeleteById(id, "fake-user-id");

        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task GetUserBoards_should_call_board_repository()
    {
        var expected = new List<BoardListItem>();

        _boardRepositoryMock.Setup(x => x.GetUserBoards("fake-user-id", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.GetUserBoards("fake-user-id");
        
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task AddChores_should_call_board_repository()
    {
        var input = new BoardChoresInput();
        var expected = new Board();

        _boardRepositoryMock.Setup(x => x.AddChores(input, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.AddChores(input);

        result.Should().Be(expected);
    }
    
    [Test]
    public async Task RemoveChores_should_call_board_repository()
    {
        var input = new BoardChoresInput();
        var expected = new Board();

        _boardRepositoryMock.Setup(x => x.RemoveChores(input, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.RemoveChores(input);

        result.Should().Be(expected);
    }

    [Test]
    public async Task AddCollaborators_should_call_board_repository()
    {
        var input = new BoardCollaboratorsInput();
        var expected = new Board();

        _boardRepositoryMock.Setup(x => x.AddCollaborators(input, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        var result = await _sut.AddCollaborators(input);

        result.Should().Be(expected);
    }
}