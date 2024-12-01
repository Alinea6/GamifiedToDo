using GamifiedToDo.Adapters.Data;
using GamifiedToDo.Adapters.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GamifiedToDo.Tests.Unit.Adapters;

public static class FakeDbContextProvider
{
    public static DataContext GetFakeDbContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase($"FakeDatabase_{Guid.NewGuid()}")
            .Options;
        var context = new DataContext(options);

        var chores = GenerateChores();
        var users = GenerateUsers();
        var userLevels = GenerateUserLevels();
        var boards = GenerateBoards(chores, users);
        
        context.Chores.AddRange(chores);
        context.Users.AddRange(users);
        context.UserLevels.AddRange(userLevels);
        context.Boards.AddRange(boards);
        context.SaveChangesAsync();
        
        return context;
    }

    private static Chore[] GenerateChores()
    {
        return
        [
            new Chore
            {
                Id = "fake-chore-id-1",
                Status = "ToDo",
                UserId = "fake-user-5",
                ChoreText = "fake-chore-text-1",
                Difficulty = 1,
                Category = 1
            },
            new Chore
            {
                Id = "fake-chore-id-2",
                Status = "Done",
                UserId = "fake-user-6",
                ChoreText = "fake-chore-text-2",
                Difficulty = 2,
                Category = 2
            },
            new Chore
            {
                Id = "fake-chore-id-3",
                Status = "ToDo",
                UserId = "fake-user-5",
                ChoreText = "fake-chore-text-3",
                Difficulty = 3,
                Category = 3
            }
        ];
    }
    
    private static User[] GenerateUsers()
    {
        return
        [
            new User
            {
                Id = "fake-user-5",
                Login = "fake-login-1",
                Password = "fake-password-1",
                Email = "fake-email-1",
            },
            new User
            {
                Id = "fake-user-6",
                Login = "fake-login-2",
                Password = "fake-password-2",
                Email = "fake-email-2"
            },
            new User
            {
                Id = "fake-user-7",
                Login = "fake-login-3",
                Password = "fake-password-3",
                Email = "fake-email-3"
            }
        ];
    }

    private static UserLevel[] GenerateUserLevels()
    {
        return
        [
            new UserLevel
            {
                Id = "fake-user-5",
                Exp = 1
            },
            new UserLevel
            {
                Id = "fake-user-6",
                Exp = 2
            }
        ];
    }

    private static Board[] GenerateBoards(Chore[] chores, User[] users)
    {
        return
        [
            new Board
            {
                Id = "fake-board-1",
                UserId = "fake-user-5",
                Collaborators = new List<User> {users[2]},
                Chores = new List<Chore>{chores[0]},
                Name = "fake-board-name-1"
            },
            new Board
            {
                Id = "fake-board-2",
                UserId = "fake-user-6",
                Collaborators = [],
                Chores = new List<Chore>{chores[1]},
                Name = "fake-board-name-2"
            },
            new Board
            {
                Id = "fake-board-3",
                UserId = "fake-user-7",
                Collaborators = [],
                Chores = [],
                Name = "fake-board-name-3"
            }
        ];
    }
}