using System.Security.Claims;
using FluentAssertions;
using GamifiedToDo.API.Controllers;
using GamifiedToDo.Services.App.Int.Boards;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Unit.Host.Controllers;

public class BoardControllerTest
{
    private const string UserId = "fake-user-id";
    private BoardController _sut;
    private Mock<IBoardService> _boardServiceMock;

    [SetUp]
    public void SetUp()
    {
        _boardServiceMock = new Mock<IBoardService>(MockBehavior.Strict);
        _sut = new BoardController(_boardServiceMock.Object);
        
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
        _boardServiceMock.VerifyAll();
    }

    [Test]
    public async Task GetById_should_call_board_service_and_return_board()
    {
        // arrange
        var expected = new Board();

        _boardServiceMock.Setup(x => x.GetById("fake-board-id", UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        // act
        var result = await _sut.GetById("fake-board-id");
        
        // assert
        result.Should().Be(expected);
    }
}