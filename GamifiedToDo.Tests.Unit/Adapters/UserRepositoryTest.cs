using System.IdentityModel.Tokens.Jwt;
using FluentAssertions;
using GamifiedToDo.Adapters.Data;
using GamifiedToDo.Adapters.Data.Repositories;
using GamifiedToDo.Services.App.Int.Users;
using Microsoft.EntityFrameworkCore;
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

    [Test]
    public async Task GetUsers_should_return_all_users_when_search_string_is_empty()
    {
        var input = new GetUsersInput();
        
        // act
        var result = await _sut.GetUsers(input);

        result.Count().Should().Be(3);
    }

    [Test]
    public async Task GetUsers_should_return_users_that_match_search_string()
    {
        var input = new GetUsersInput
        {
            Search = "2"
        };
        
        var result = await _sut.GetUsers(input);

        var enumerable = result as GamifiedToDo.Services.App.Int.Users.User[] ?? result.ToArray();
        
        enumerable.Count().Should().Be(1);
        enumerable.First().Login.Should().Be("fake-login-2");
    }

    [Test]
    public async Task GetUsers_should_return_users_after_pagination()
    {
        var input = new GetUsersInput
        {
            PageSize = 2,
            PageNumber = 2
        };
        
        var result = await _sut.GetUsers(input);
        
        var enumerable = result as GamifiedToDo.Services.App.Int.Users.User[] ?? result.ToArray();
        
        enumerable.Count().Should().Be(1);
        enumerable.First().Login.Should().Be("fake-login-3");
    }

    [Test]
    public async Task RemoveFriend_should_remove_friend_and_not_throw_error()
    {
        var user1 = await _context.Users.Include(x=> x.Friends)
            .FirstAsync(x => x.Id == "fake-user-5");
        var user2 = await _context.Users.Include(x=> x.Friends)
            .FirstAsync(x => x.Id == "fake-user-6");
        user1.Friends.Add(user2);
        await _context.SaveChangesAsync();
        
        var act = () => _sut.RemoveFriend("fake-user-5", "fake-user-6");

        await act.Should().NotThrowAsync();
        user1.Friends.Count.Should().Be(0);
    }
    
    [Test]
    public async Task RemoveFriend_should_throw_error_when_user_does_not_exist()
    {
        
        var act = () => _sut.RemoveFriend("fake-user-99", "fake-user-6");

        await act.Should().ThrowAsync<Exception>().WithMessage("Couldn't find user with id fake-user-99");
    }
    
    [Test]
    public async Task RemoveFriend_should_throw_error_when_user_does_not_have_friend_with_given_id()
    {
        var act = () => _sut.RemoveFriend("fake-user-5", "fake-user-6");

        await act.Should().ThrowAsync<Exception>().WithMessage("User does not have friend with id fake-user-6");
    }
    
    [Test]
    public async Task GetUserFriends_should_find_user_and_return_UserFriends()
    {
        var expected = new UserFriends
        {
            UserId = "fake-user-5",
            Friends = new List<GamifiedToDo.Services.App.Int.Users.User>
            {
                new()
                {
                    Id = "fake-user-6",
                    Login = "fake-login-2"
                }
            }
        };
        
        var user1 = await _context.Users.Include(x=> x.Friends)
            .FirstAsync(x => x.Id == "fake-user-5");
        var user2 = await _context.Users.Include(x=> x.Friends)
            .FirstAsync(x => x.Id == "fake-user-6");
        user1.Friends.Add(user2);
        await _context.SaveChangesAsync();
        
        var result = await _sut.GetUserFriends("fake-user-5");

        result.Should().BeEquivalentTo(expected);
    }
}