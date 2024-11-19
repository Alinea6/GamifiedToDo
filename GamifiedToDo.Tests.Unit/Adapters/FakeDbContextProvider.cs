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
        
        context.Chores.AddRange(chores);
        context.Users.AddRange(users);
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
                ChoreText = "fake-chore-text-1"
            },
            new Chore
            {
                Id = "fake-chore-id-2",
                Status = "Done",
                UserId = "fake-user-6",
                ChoreText = "fake-chore-text-2"
            },
            new Chore
            {
                Id = "fake-chore-id-3",
                Status = "ToDo",
                UserId = "fake-user-5",
                ChoreText = "fake-chore-text-3"
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
            }
        ];
    }
}