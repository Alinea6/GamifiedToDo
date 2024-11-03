using FluentAssertions;
using GamifiedToDo.Services.App.Chores;
using GamifiedToDo.Services.App.Dep.Chores;
using GamifiedToDo.Services.App.Int;
using Moq;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Unit.Services;

public class ChoreServiceTest
{
    private ChoreService _sut;
    private Mock<IChoreRepository> _choreRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        _choreRepositoryMock = new Mock<IChoreRepository>(MockBehavior.Strict);
        _sut = new ChoreService(_choreRepositoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _choreRepositoryMock.VerifyAll();
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
}