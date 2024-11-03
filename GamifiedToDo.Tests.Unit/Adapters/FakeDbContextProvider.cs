using GamifiedToDo.Adapters.Data;
using GamifiedToDo.Adapters.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GamifiedToDo.Tests.Unit.Adapters;

public class FakeDbContextProvider
{
    public static DataContext GetFakeDbContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase($"FakeDatabase_{Guid.NewGuid()}")
            .Options;
        var context = new DataContext(options);

        var chores = GenerateChores();
        
        context.Chores.AddRange(chores);
        context.SaveChangesAsync();
        
        return context;
    }

    private static Chore[] GenerateChores()
    {
        return
        [
            new Chore
            {
                Id = "fake-id-1",
                UserId = "fake-user-1",
                ChoreText = "fake-chore-text-1",
                Status = "ToDo"
            },
            new Chore
            {
                Id = "fake-id-2",
                UserId = "fake-user-1",
                ChoreText = "fake-chore-text-2",
                Status = "Done"
            },
            new Chore
            {
                Id = "fake-id-3",
                UserId = "fake-user-2",
                ChoreText = "fake-chore-text-2",
                Status = "fake-status"
            }
        ];
    }
}