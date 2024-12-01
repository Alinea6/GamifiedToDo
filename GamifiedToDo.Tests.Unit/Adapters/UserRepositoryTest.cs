using System.IdentityModel.Tokens.Jwt;
using FluentAssertions;
using GamifiedToDo.Adapters.Data;
using GamifiedToDo.Adapters.Data.Repositories;
using GamifiedToDo.Services.App.Int.Users;
using Moq;
using NUnit.Framework;
using User = GamifiedToDo.Adapters.Data.Models.User;

namespace GamifiedToDo.Tests.Unit.Adapters;

public class UserRepositoryTest
{
    private UserRepository _sut;
    private DataContext _context;
    private Mock<IJsonWebTokenProvider> _jwtProviderMock;
    
    [SetUp]
    public void SetUp()
    {
        _context = FakeDbContextProvider.GetFakeDbContext();
        _jwtProviderMock = new Mock<IJsonWebTokenProvider>(MockBehavior.Strict);
        _sut = new UserRepository(_context, _jwtProviderMock.Object);
    }
    
    [TearDown]
    public void TearDown()
    {
        _jwtProviderMock.VerifyAll();
    }
    
    [Test]
    public async Task Register_should_create_user_and_return_user_id()
    {
        // arrange
        var originalContextUsersCount = _context.Users.Count();
        var input = new RegisterInput
        {
            Login = "fake-login-3",
            Password = "fake-password-3",
            Email = "fake-email-3",
        };
        
        // act
        var result = await _sut.Register(input);
        
        // assert
        _context.Users.Count().Should().Be(originalContextUsersCount + 1);
    }
    
    [Test]
    public async Task Login_should_find_user_with_login_and_password_generate_token_and_return_it()
    {
        // arrange
        var user = new User
        {
            Id = "fake-user-5",
            Login = "fake-login-1",
            Password = "fake-password-1",
            Email = "fake-email-1",
        };
        var token = new JwtSecurityToken();

        _jwtProviderMock.Setup(x => x.GenerateToken(user.Id))
            .Returns(token);
        
        // act
        var result = await _sut.Login(new LoginInput
        {
            Login = "fake-login-1",
            Password = "fake-password-1"
        });
        
        // assert
        result.Should().NotBeEmpty();
    }
    
    [Test]
    public async Task Login_should_throw_error_when_login_or_password_is_incorrect()
    {
        // act
        var act = () => _sut.Login(new LoginInput
        {
            Login = "fake-login-99",
            Password = "fake-password-99"
        });
        
        // assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Incorrect login or password");
    }
}