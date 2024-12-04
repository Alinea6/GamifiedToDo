using FluentAssertions;
using GamifiedToDo.Adapters.Data;
using GamifiedToDo.Adapters.Data.Repositories;
using GamifiedToDo.Services.App.Int.Boards;
using GamifiedToDo.Services.App.Int.Users;
using NUnit.Framework;
using Board = GamifiedToDo.Services.App.Int.Boards.Board;

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
            Owner = new User
            {
                Id = "fake-user-5",
                Login = "fake-login-1",
            },
            Name = "fake-board-name-1",
            IsOwner = true,
            Collaborators = new List<User>
            {
                new()
                {
                    Id = "fake-user-7",
                    Login = "fake-login-3",
                }
            }
        };

        var result = await _sut.GetById("fake-board-1", "fake-user-5");
        
        result.Id.Should().Be("fake-board-1");
        result.Owner.Id.Should().Be("fake-user-5");
    }

    [Test]
    public async Task GetById_should_get_board_when_user_is_collaborator()
    {
        var expected = new Board
        {
            Id = "fake-board-1",
            Owner = new User
            {
                Id = "fake-user-5",
                Login = "fake-login-1",
            },
            Name = "fake-board-name-1",
            IsOwner = false,
            Collaborators = new List<User>
            {
                new()
                {
                    Id = "fake-user-7",
                    Login = "fake-login-3",
                }
            }
        };

        var result = await _sut.GetById("fake-board-1", "fake-user-7");

        result.Id.Should().BeEquivalentTo("fake-board-1");
        result.Collaborators.First().Id.Should().Be("fake-user-7");
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
            UserId = "fake-user-7",
            Name = "fake-board-name",
            Collaborators = new List<string> { "fake-user-6", "fake-user-5" },
            ChoreIds = new List<string> { "fake-chore-1", "fake-chore-2" }
        };
        // act
         var result = await _sut.Add(input);
        
        // assert
        result.Name.Should().Be(input.Name);
        result.Owner.Id.Should().Be(input.UserId);
        _context.Boards.Count().Should().Be(originalBoardCount + 1);
    }

    [Test]
    public async Task DeleteById_should_remove_board()
    {
        var originalBoardCount = _context.Boards.Count();
        
        // act
        var act = () => _sut.DeleteById("fake-board-3", "fake-user-7");

        await act.Should().NotThrowAsync();
        _context.Boards.Count().Should().Be(originalBoardCount - 1);
    }

    [Test]
    public async Task GetUserBoards_should_get_all_the_user_boards_map_them_and_return_them()
    {
        var expected = new List<BoardListItem>
        {
            new()
            {
                Id = "fake-board-1",
                Owner = new User
                {
                    Id = "fake-user-5",
                    Login = "fake-login-1"
                },
                Name = "fake-board-name-1",
                Collaborators = new List<User>
                {
                    new()
                    {
                        Id = "fake-user-7",
                        Login = "fake-login-3",
                    }
                },
                IsOwner = false
            },
            new()
            {
                Id = "fake-board-3",
                Owner = new User
                {
                    Id = "fake-user-7",
                    Login = "fake-login-3",
                },
                Name = "fake-board-name-3",
                Collaborators = new List<User>(),
                IsOwner = true
            }
        };

        var result = await _sut.GetUserBoards("fake-user-7");

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task AddChores_should_add_chores_to_board()
    {
        var input = new BoardChoresInput
        {
            Id = "fake-board-3",
            UserId = "fake-user-7",
            ChoreIds = new List<string>
            {
                "fake-chore-id-4",
                "fake-chore-id-5"
            }
        };

        var result = await _sut.AddChores(input);

        result.Chores.Count().Should().Be(2);
        result.Id.Should().Be("fake-board-3");
    }

    [Test]
    public async Task RemoveChores_should_remove_chores_from_board_by_owner()
    {
        var input = new BoardChoresInput
        {
            Id = "fake-board-2",
            UserId = "fake-user-6",
            ChoreIds = new List<string>
            {
                "fake-chore-id-2"
            }
        };
        
        var result = await _sut.RemoveChores(input);

        result.Chores.Count().Should().Be(0);
    }
    
    [Test]
    public async Task RemoveChores_should_remove_chores_from_board_by_collaborator()
    {
        var input = new BoardChoresInput
        {
            Id = "fake-board-1",
            UserId = "fake-user-7",
            ChoreIds = new List<string>
            {
                "fake-chore-id-1"
            }
        };
        
        var result = await _sut.RemoveChores(input);

        result.Chores.Count().Should().Be(0);
    }

    [Test]
    public async Task AddCollaborators_should_add_collaborators_to_the_board()
    {
        var input = new BoardCollaboratorsInput()
        {
            Id = "fake-board-3",
            UserId = "fake-user-7",
            CollaboratorIds = new List<string>
            {
                "fake-user-5"
            }
        };

        var result = await _sut.AddCollaborators(input);

        result.Collaborators.Count().Should().Be(1);
        result.Collaborators.First().Id.Should().Be("fake-user-5");
    }

    [Test]
    public async Task RemoveCollaborators_should_remove_collaborators_from_the_board()
    {
        var input = new BoardCollaboratorsInput
        {
            Id = "fake-board-1",
            UserId = "fake-user-5",
            CollaboratorIds = new List<string>
            {
                "fake-user-7"
            }
        };

        var result = await _sut.RemoveCollaborators(input);

        result.Collaborators.Should().BeEmpty();
    }
}