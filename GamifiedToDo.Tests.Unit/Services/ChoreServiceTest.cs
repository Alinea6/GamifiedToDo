using FluentAssertions;
using GamifiedToDo.Services.App.Chores;
using GamifiedToDo.Services.App.Dep.Chores;
using GamifiedToDo.Services.App.Int;
using GamifiedToDo.Services.App.Int.Chores;
using GamifiedToDo.Services.App.Int.UserLevels;
using GamifiedToDo.Tests.Unit.Helpers;
using Moq;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Unit.Services;

public class ChoreServiceTest
{
    private ChoreService _sut;
    private Mock<IChoreRepository> _choreRepositoryMock;
    private Mock<IUserLevelService> _userLevelServiceMock;

    [SetUp]
    public void SetUp()
    {
        _choreRepositoryMock = new Mock<IChoreRepository>(MockBehavior.Strict);
        _userLevelServiceMock = new Mock<IUserLevelService>(MockBehavior.Strict);
        _sut = new ChoreService(_choreRepositoryMock.Object, _userLevelServiceMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _choreRepositoryMock.VerifyAll();
        _userLevelServiceMock.VerifyAll();
    }

    [Test]
    public async Task GetUserChores_should_call_repository_and_return_list_of_chores()
    {
        // arrange
        var userId = "fake-user-id";
        var expected = new List<Chore>
        {
            new()
            {
                Id = "fake-chore-id"
            }
        };

        _choreRepositoryMock.Setup(x => x.GetUserChores(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        // act
        var result = await _sut.GetUserChores(userId);
        
        // assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetChoreById_should_call_chore_repository_and_return_chore()
    {
        // arrange
        var choreId = "fake-chore-id";
        var userId = "fake-user-id";
        var expected = new Chore();

        _choreRepositoryMock.Setup(x => x.GetChoreById(choreId, userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        // act
        var result = await _sut.GetChoreById(choreId, userId);
        
        // assert
        result.Should().Be(expected);
    }

    [Test]
    public async Task AddChore_should_call_chore_repository_and_return_chore()
    {
        // arrange
        var input = new ChoreAddInput();
        var expected = new Chore();

        _choreRepositoryMock.Setup(x => x.AddChore(input, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.AddChore(input);

        result.Should().Be(expected);
    }

    [Test]
    public async Task DeleteChoreById_should_call_chore_repository()
    {
        _choreRepositoryMock.Setup(x => x.DeleteChoreById(
                "fake-chore-id",
                "fake-user-id",
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var act = () => _sut.DeleteChoreById("fake-chore-id", "fake-user-id");

        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task UpdateChoreById_should_call_chore_repository()
    {
        var input = new ChoreUpdateInput();
        var expected = new Chore();

        _choreRepositoryMock.Setup(x => x.UpdateChoreById(input, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.UpdateChoreById(input);

        result.Should().Be(expected);
    }
    
    [Test]
    public async Task UpdateStatusById_should_call_chore_repository_and_user_level_service()
    {
        var input = new ChoreUpdateStatusInput
        {
            UserId = "fake-user-id"
        };
        var expected = new Chore
        {
            Difficulty = ChoreDifficulty.Simple
        };

        _choreRepositoryMock.Setup(x => x.UpdateStatusById(MoqHandler.IsEquivalentTo(input), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        _userLevelServiceMock.Setup(x => x.UpdateExp(
                input.UserId,
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(10);
            

        var result = await _sut.UpdateStatusById(input);

        result.Should().BeEquivalentTo(expected);
    }
}