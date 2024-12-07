using FluentAssertions;
using GamifiedToDo.Adapters.Data;
using GamifiedToDo.Adapters.Data.Repositories;
using GamifiedToDo.Services.App.Int.Chores;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Unit.Adapters;

public class ChoreRepositoryTest
{
    private ChoreRepository _sut;
    private DataContext _context;

    [SetUp]
    public void SetUp()
    {
        _context = FakeDbContextProvider.GetFakeDbContext();
        _sut = new ChoreRepository(_context);
    }

    [Test]
    public async Task GetUserChores_should_get_all_chores_with_specific_user_id()
    {
        // arrange
        var expected = new List<Chore>
        {
            new()
            {
                Id = "fake-chore-id-1",
                Status = ChoreStatus.ToDo,
                UserId = "fake-user-5",
                ChoreText = "fake-chore-text-1",
                Difficulty = ChoreDifficulty.Simple,
                Category = ChoreCategory.SelfImprovement
            },
            new()
            {
                Id = "fake-chore-id-3",
                Status = ChoreStatus.ToDo,
                UserId = "fake-user-5",
                ChoreText = "fake-chore-text-3",
                Difficulty = ChoreDifficulty.Complex,
                Category = ChoreCategory.Cleaning
            }
        };
        
        // act
        var result = await _sut.GetUserChores("fake-user-5");
        
        // assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetUserChores_should_return_empty_list_if_user_has_no_chores()
    {
        var result = await _sut.GetUserChores("fake-user-99");
        
        result.Should().BeEquivalentTo(new List<Chore>());
    }

    [Test]
    public async Task GetChoreById_should_throw_exception_if_chore_is_not_found()
    {
        var act = () => _sut.GetChoreById("fake-id-99", "fake-user-99");

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Chore with id fake-id-99 for user fake-user-99 was not found");
    }

    [Test]
    public async Task GetChoreById_should_return_and_map_correct_chore()
    {
        var expected = new Chore
        {
            Id = "fake-chore-id-1",
            UserId = "fake-user-5",
            ChoreText = "fake-chore-text-1",
            Status = ChoreStatus.ToDo,
            Difficulty = ChoreDifficulty.Simple,
            Category = ChoreCategory.SelfImprovement
        };
        
        var result = await _sut.GetChoreById("fake-chore-id-1", "fake-user-5");

        result.Should().BeEquivalentTo(expected);
    }

    [TestCase("fake-chore-id-1", "fake-user-5", ChoreStatus.ToDo)]
    [TestCase("fake-chore-id-2", "fake-user-6", ChoreStatus.Done)]
    [TestCase("fake-chore-id-3", "fake-user-5", ChoreStatus.ToDo)]
    public async Task GetChoreById_should_return_correctly_mapped_chore_status(
        string choreId, 
        string userId, 
        ChoreStatus expectedStatus)
    {
        var result = await _sut.GetChoreById(choreId, userId);

        result.Status.Should().Be(expectedStatus);
    }

    [Test]
    public async Task Add_should_create_chore_and_return_it()
    {
        // arrange
        var originalChoreCount = _context.Chores.Count();
        var input = new ChoreAddInput
        {
            UserId = "fake-id-5",
            ChoreText = "fake-chore-text",
            Status = ChoreStatus.ToDo,
            Difficulty = ChoreDifficulty.Moderate,
            Category = ChoreCategory.Education
        };
        
        // act
        var result = await _sut.AddChore(input);
        
        // assert
        result.UserId.Should().Be(input.UserId);
        result.ChoreText.Should().Be(input.ChoreText);
        result.Status.Should().Be(input.Status);
        result.Difficulty.Should().Be(input.Difficulty);
        result.Category.Should().Be(input.Category);
        _context.Chores.Count().Should().Be(originalChoreCount + 1);
    }

    [Test]
    public async Task DeleteChoreById_should_remove_chore()
    {
        var originalChoreCount = _context.Chores.Count();
        
        var act = () => _sut.DeleteChoreById("fake-chore-id-1", "fake-user-5");

        await act.Should().NotThrowAsync();
        _context.Chores.Count().Should().Be(originalChoreCount - 1);

    }
    
    [Test]
    public async Task DeleteChoreById_should_throw_exception_if_chore_is_not_found()
    {
        var act = () => _sut.DeleteChoreById("fake-id-99", "fake-user-99");

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Chore with id fake-id-99 for user fake-user-99 was not found");
    }

    [Test]
    public async Task UpdateChoreById_should_update_chore()
    {
        var input = new ChoreUpdateInput
        {
            Id = "fake-chore-id-1",
            UserId = "fake-user-5",
            ChoreText = "new-fake-text",
            Difficulty = ChoreDifficulty.Moderate,
            Category = ChoreCategory.Education
        };
        
        // act
        var result = await _sut.UpdateChoreById(input);
        
        // assert
        result.ChoreText.Should().Be(input.ChoreText);
        result.Difficulty.Should().Be(input.Difficulty);
        result.Category.Should().Be(input.Category);
    }
    
    [Test]
    public async Task UpdateChoreById_should_throw_exception_if_chore_is_not_found()
    {
        var act = () => _sut.UpdateChoreById(new ChoreUpdateInput
        {
            Id = "fake-id-99",
            UserId = "fake-user-99",
        });

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Chore with id fake-id-99 for user fake-user-99 was not found");
    }

    [Test]
    public async Task UpdateStatusById_should_update_chore_status()
    {
        var input = new ChoreUpdateStatusInput
        {
            Id = "fake-chore-id-1",
            UserId = "fake-user-5",
            Status = ChoreStatus.Done
        };
        var expected = new Chore
        {
            Id = "fake-chore-id-1",
            UserId = "fake-user-5",
            ChoreText = "fake-chore-text-1",
            Status = ChoreStatus.Done,
            Difficulty = ChoreDifficulty.Simple,
            Category = ChoreCategory.SelfImprovement
        };
        
        // act
        var result = await _sut.UpdateStatusById(input);
        
        // assert
        result.Should().BeEquivalentTo(expected);
    }
    
    [Test]
    public async Task UpdateStatusById_should_throw_exception_if_chore_is_not_found()
    {
        var act = () => _sut.UpdateStatusById(new ChoreUpdateStatusInput
        {
            Id = "fake-id-99",
            UserId = "fake-user-99",
            Status = ChoreStatus.Done
        });

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Chore with id fake-id-99 for user fake-user-99 was not found");
    }
    
    [Test]
    public async Task UpdateStatusById_should_update_chore_status_by_collaborator_id()
    {
        var input = new ChoreUpdateStatusInput
        {
            Id = "fake-chore-id-1",
            UserId = "fake-user-7",
            Status = ChoreStatus.Done
        };
        var expected = new Chore
        {
            Id = "fake-chore-id-1",
            UserId = "fake-user-5",
            ChoreText = "fake-chore-text-1",
            Status = ChoreStatus.Done,
            Difficulty = ChoreDifficulty.Simple,
            Category = ChoreCategory.SelfImprovement
        };
        
        // act
        var result = await _sut.UpdateStatusById(input);
        
        // assert
        result.Should().BeEquivalentTo(expected);
    }
}