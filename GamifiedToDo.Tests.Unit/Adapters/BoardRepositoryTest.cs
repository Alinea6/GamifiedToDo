using FluentAssertions;
using GamifiedToDo.Adapters.Data;
using GamifiedToDo.Adapters.Data.Repositories;
using GamifiedToDo.Services.App.Int.Boards;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Unit.Adapters;

public class BoardRepositoryTest
{
    private BoardRepository _sut;
    private DataContext _context;

    [SetUp]
    public void SetUp()
    {
        _context = FakeDbContextProvider.GetFakeDbContext();
        _sut = new BoardRepository(_context);
    }

    [Test]
    public async Task GetById_should_get_board_when_user_is_board_owner_and_map_it()
    {
        var expected = new Board
        {
            Id = "fake-board-1",
            UserId = "fake-user-5",
            Collaborators = new List<string> { "fake-user-7" },
            ChoreIds = new List<string> { "fake-chore-id-1" },
            Name = "fake-board-name-1"
        };

        var result = await _sut.GetById("fake-board-1", "fake-user-5");
        
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetById_should_get_board_when_user_is_collaborator()
    {
        var expected = new Board
        {
            Id = "fake-board-1",
            UserId = "fake-user-5",
            Collaborators = new List<string> { "fake-user-7" },
            ChoreIds = new List<string> { "fake-chore-id-1" },
            Name = "fake-board-name-1"
        };

        var result = await _sut.GetById("fake-board-1", "fake-user-7");
        
        result.Should().BeEquivalentTo(expected);
    }
    
    [Test]
    public async Task GetById_should_throw_exception_if_board_is_not_found()
    {
        var act = () => _sut.GetById("fake-id-99", "fake-user-99");

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Board with id fake-id-99 for user fake-user-99 was not found");
    }

    [Test]
    public async Task Add_should_add_board_and_return_it()
    {
        var originalBoardCount = _context.Boards.Count();
        var input = new BoardAddInput
        {
            UserId = "fake-user-9",
            Name = "fake-board-name",
            Collaborators = new List<string> { "fake-user-10", "fake-user-11" },
            ChoreIds = new List<string> { "fake-chore-1", "fake-chore-2" }
        };
        
        // act
        var result = await _sut.Add(input);
        
        // assert
        result.UserId.Should().Be(input.UserId);
        result.Name.Should().Be(input.Name);
        result.ChoreIds.Should().BeEquivalentTo(input.ChoreIds);
        result.Collaborators.Should().BeEquivalentTo(input.Collaborators);
        _context.Boards.Count().Should().Be(originalBoardCount + 1);
    }
}