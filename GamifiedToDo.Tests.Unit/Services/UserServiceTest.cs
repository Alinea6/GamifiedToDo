using FluentAssertions;
using GamifiedToDo.Services.App.Dep.FriendRequests;
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
    private Mock<IFriendRequestRepository> _friendRequestRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        _friendRequestRepositoryMock = new Mock<IFriendRequestRepository>(MockBehavior.Strict);
        _sut = new UserService(_userRepositoryMock.Object, _friendRequestRepositoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _userRepositoryMock.VerifyAll();
        _friendRequestRepositoryMock.VerifyAll();
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
        var input = new GetUsersInput();
        var expected = new List<User>();

        _userRepositoryMock.Setup(x => x.GetUsers(input, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.GetUsers(input);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task CreateFriendRequest_should_call_friend_request_repository()
    {
        var input = new FriendRequestInput();

        _friendRequestRepositoryMock.Setup(x => x.CreateFriendRequest(input, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        var act = () => _sut.CreateFriendRequest(input);

        await act.Should().NotThrowAsync();
    }
    
    [Test]
    public async Task AcceptFriendRequest_should_call_friend_request_repository()
    {
        var input = new FriendRequestInput();

        _friendRequestRepositoryMock.Setup(x => x.AcceptFriendRequest(input, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        var act = () => _sut.AcceptFriendRequest(input);

        await act.Should().NotThrowAsync();
    }
    
    [Test]
    public async Task RemoveFriendRequest_should_call_friend_request_repository()
    {
        var requestId = "fake-request-id";
        var userId = "fake-user-id";

        _friendRequestRepositoryMock.Setup(x => x.RemoveFriendRequest(
                requestId, 
                userId, 
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        var act = () => _sut.RemoveFriendRequest(requestId, userId);

        await act.Should().NotThrowAsync();
    }
    
    [Test]
    public async Task RemoveFriend_should_call_user_repository()
    {
        var friendId = "fake-friend-id";
        var userId = "fake-user-id";

        _userRepositoryMock.Setup(x => x.RemoveFriend(
                userId, 
                friendId, 
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        var act = () => _sut.RemoveFriend(userId, friendId);

        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task GetUserFriends_should_call_user_repository_and_return_user_friends()
    {
        var expected = new UserFriends();

        _userRepositoryMock.Setup(x => x.GetUserFriends("fake-user-id", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.GetUserFriends("fake-user-id");

        result.Should().Be(expected);
    }
}