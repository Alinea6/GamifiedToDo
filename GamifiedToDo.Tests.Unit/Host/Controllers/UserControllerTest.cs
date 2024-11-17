using FluentAssertions;
using GamifiedToDo.API.Controllers;
using GamifiedToDo.API.Models;
using GamifiedToDo.Services.App.Int.Users;
using GamifiedToDo.Tests.Unit.Helpers;
using Moq;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Unit.Host.Controllers;

public class UserControllerTest
{
    private UserController _sut;
    private Mock<IUserService> _userServiceMock;

    [SetUp]
    public void SetUp()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
        _sut = new UserController(_userServiceMock.Object);
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
}