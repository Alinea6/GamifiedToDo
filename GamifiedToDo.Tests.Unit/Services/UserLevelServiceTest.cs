using FluentAssertions;
using GamifiedToDo.Services.App.Dep.UserLevels;
using GamifiedToDo.Services.App.UserLevels;
using Moq;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Unit.Services;

public class UserLevelServiceTest
{
    private UserLevelService _sut;
    private Mock<IUserLevelRepository> _userLevelRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        _userLevelRepositoryMock = new Mock<IUserLevelRepository>(MockBehavior.Strict);
        _sut = new UserLevelService(_userLevelRepositoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _userLevelRepositoryMock.VerifyAll();
    }

    [Test]
    public async Task UpdateExp_should_calculate_exp_and_pass_it_to_repository()
    {
        var userId = "fake-user-id";
        var choreDifficulty = 1;
        var experienceResult = 5;
        var expected = 200;

        _userLevelRepositoryMock.Setup(x => x.UpdateExp(
                userId,
                experienceResult,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.UpdateExp(userId, choreDifficulty);

        result.Should().Be(expected);
    }
}