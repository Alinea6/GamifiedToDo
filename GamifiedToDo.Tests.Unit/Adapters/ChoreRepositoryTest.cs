using FluentAssertions;
using GamifiedToDo.Adapters.Data;
using GamifiedToDo.Adapters.Data.Repositories;
using GamifiedToDo.Services.App.Int;
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
                Id = "fake-id-1",
                UserId = "fake-user-1",
                ChoreText = "fake-chore-text-1",
                Status = ChoreStatus.ToDo
            },
            new()
            {
                Id = "fake-id-2",
                UserId = "fake-user-1",
                ChoreText = "fake-chore-text-2",
                Status = ChoreStatus.Done
            }
        };
        
        // act
        var result = await _sut.GetUserChores("fake-user-1");
        
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
            Id = "fake-id-1",
            UserId = "fake-user-1",
            ChoreText = "fake-chore-text-1",
            Status = ChoreStatus.ToDo
        };
        
        var result = await _sut.GetChoreById("fake-id-1", "fake-user-1");

        result.Should().BeEquivalentTo(expected);
    }

    [TestCase("fake-id-1", "fake-user-1", ChoreStatus.ToDo)]
    [TestCase("fake-id-2", "fake-user-1", ChoreStatus.Done)]
    [TestCase("fake-id-3", "fake-user-2", ChoreStatus.ToDo)]
    public async Task GetChoreById_should_return_correctly_mapped_chore_status(
        string choreId, 
        string userId, 
        ChoreStatus expectedStatus)
    {
        var result = await _sut.GetChoreById(choreId, userId);

        result.Status.Should().Be(expectedStatus);
    }
}