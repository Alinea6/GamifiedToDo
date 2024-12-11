using System.Security.Claims;
using FluentAssertions;
using GamifiedToDo.API.Controllers;
using GamifiedToDo.API.Models;
using GamifiedToDo.API.Models.Users;
using GamifiedToDo.Services.App.Int.Users;
using GamifiedToDo.Tests.Unit.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Unit.Host.Controllers;

public class UserControllerTest
{
    private UserController _sut;
    private Mock<IUserService> _userServiceMock;
    private const string UserId = "fake-user-id";

    [SetUp]
    public void SetUp()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
        _sut = new UserController(_userServiceMock.Object);
        
        var context = new DefaultHttpContext();

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, "fake-user-id"),
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        context.User = claimsPrincipal;

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };
    }

    [TearDown]
    public void TearDown()
    {
        _userServiceMock.VerifyAll();
    }

    [Test]
    public async Task Register_should_map_request_to_input_call_user_service_and_return_user_id()
    {
        // arrange
        var request = new RegisterRequest
        {
            Login = "fake-login",
            Password = "fake-password",
            Email = "fake-email",
        };
        var input = new RegisterInput
        {
            Login = "fake-login",
            Password = "fake-password",
            Email = "fake-email",
        };

        _userServiceMock.Setup(x => x.Register(MoqHandler.IsEquivalentTo(input), It.IsAny<CancellationToken>()))
            .ReturnsAsync("fake-id");
        
        // act
        var result = await _sut.Register(request);
        
        // assert
        result.Should().Be("fake-id");
    }
    
    [Test]
    public async Task Login_should_map_request_to_input_call_user_service_and_return_token()
    {
        // arrange
        var request = new LoginRequest
        {
            Login = "fake-login",
            Password = "fake-password"
        };
        var input = new LoginInput
        {
            Login = "fake-login",
            Password = "fake-password"
        };

        _userServiceMock.Setup(x => x.Login(MoqHandler.IsEquivalentTo(input), It.IsAny<CancellationToken>()))
            .ReturnsAsync("fake-token");
        
        // act
        var result = await _sut.Login(request);
        
        // assert
        result.Should().Be("fake-token");
    }

    [Test]
    public async Task GetUsers_should_call_user_service_and_return_list_of_users()
    {
        var input = new GetUsersInput
        {
            Search = "fake-search",
            PageSize = 10,
            PageNumber = 10
        };
        var expected = new List<User>();
        
        _userServiceMock.Setup(x => x.GetUsers(
                MoqHandler.IsEquivalentTo(input), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.GetUsers("fake-search", 10, 10);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task CreateFriendRequest_should_call_user_service()
    {
        var request = new FriendRequest
        {
            FriendId = "fake-friend-id"
        };

        var input = new FriendRequestInput
        {
            UserId = UserId,
            FriendId = "fake-friend-id"
        };

        _userServiceMock.Setup(x =>
                x.CreateFriendRequest(MoqHandler.IsEquivalentTo(input), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var act = () => _sut.CreateFriendRequest(request);

        await act.Should().NotThrowAsync();
    }
    
    [Test]
    public async Task AcceptFriendRequest_should_call_user_service()
    {

        var input = new FriendRequestInput
        {
            UserId = "fake-requester-id",
            FriendId = UserId
        };

        _userServiceMock.Setup(x =>
                x.AcceptFriendRequest(MoqHandler.IsEquivalentTo(input), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var act = () => _sut.AcceptFriendRequest("fake-requester-id");

        await act.Should().NotThrowAsync();
    }
}