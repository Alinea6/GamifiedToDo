using FluentAssertions;
using GamifiedToDo.Adapters.Data;
using GamifiedToDo.Adapters.Data.Repositories;
using GamifiedToDo.Services.App.Int.Users;
using Moq;
using NUnit.Framework;

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
}