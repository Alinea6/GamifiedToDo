using FluentAssertions;
using GamifiedToDo.Services.App.Dep.Users;
using GamifiedToDo.Services.App.Int.Users;
using GamifiedToDo.Services.App.Users;
using Moq;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Unit.Services;

public class UserServiceTest
{
    private UserService _sut;
    private Mock<IUserRepository> _userRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        _sut = new UserService(_userRepositoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _userRepositoryMock.VerifyAll();
    }

    [Test]
    public async Task Register_should_call_user_repository_and_return_user_id()
    {
        // arrange
        var input = new RegisterInput();

        _userRepositoryMock.Setup(x => x.Register(input, It.IsAny<CancellationToken>()))
            .ReturnsAsync("fake-id");
        
        // act
        var result = await _sut.Register(input);

        result.Should().Be("fake-id");
    }
}