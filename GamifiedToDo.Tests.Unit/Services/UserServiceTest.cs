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
    
    [Test]
    public async Task Login_should_call_user_repository_and_return_token()
    {
        // arrange
        var input = new LoginInput();

        _userRepositoryMock.Setup(x => x.Login(input, It.IsAny<CancellationToken>()))
            .ReturnsAsync("fake-token");
        
        // act
        var result = await _sut.Login(input);
        
        // assert
        result.Should().Be("fake-token");
    }

    [Test]
    public async Task GetUsers_should_call_user_repository_and_return_list_of_users()
    {
        var expected = new List<User>();

        _userRepositoryMock.Setup(x => x.GetUsers("search-string", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.GetUsers("search-string");

        result.Should().BeEquivalentTo(expected);
    }
}