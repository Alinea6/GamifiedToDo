using FluentAssertions;
using GamifiedToDo.Adapters.Data;
using GamifiedToDo.Adapters.Data.Repositories;
using GamifiedToDo.Services.App.Int.Users;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Unit.Adapters;

public class FriendRequestRepositoryTest
{
    private FriendRequestRepository _sut;
    private DataContext _context;

    [SetUp]
    public void SetUp()
    {
        _context = FakeDbContextProvider.GetFakeDbContext();
        _sut = new FriendRequestRepository(_context);
    }

    [Test]
    public async Task CreateFriendRequest_should_add_friend_request()
    {
        var originalCount = _context.FriendRequests.Count();
        var input = new FriendRequestInput
        {
            UserId = "fake-user-6",
            FriendId = "fake-user-7"
        };

        var act = () => _sut.CreateFriendRequest(input);

        await act.Should().NotThrowAsync();
        _context.FriendRequests.Count().Should().Be(originalCount + 1);
    }
    
    [Test]
    public async Task CreateFriendRequest_should_throw_error_when_requester_does_not_exist()
    {
        var originalCount = _context.FriendRequests.Count();
        var input = new FriendRequestInput
        {
            UserId = "fake-user-99",
            FriendId = "fake-user-6"
        };

        var act = () => _sut.CreateFriendRequest(input);

        await act.Should().ThrowAsync<InvalidOperationException>();
        _context.FriendRequests.Count().Should().Be(originalCount);
    }
    
    [Test]
    public async Task CreateFriendRequest_should_throw_error_when_friend_does_not_exist()
    {
        var originalCount = _context.FriendRequests.Count();
        var input = new FriendRequestInput
        {
            UserId = "fake-user-5",
            FriendId = "fake-user-99"
        };

        var act = () => _sut.CreateFriendRequest(input);

        await act.Should().ThrowAsync<InvalidOperationException>();
        _context.FriendRequests.Count().Should().Be(originalCount);
    }
    
    [Test]
    public async Task AcceptFriendRequest_should_remove_request_and_friends_to_users()
    {
        var originalCount = _context.FriendRequests.Count();
        var input = new FriendRequestInput
        {
            UserId = "fake-user-5",
            FriendId = "fake-user-6"
        };

        var act = () => _sut.AcceptFriendRequest(input);

        await act.Should().NotThrowAsync();
        _context.FriendRequests.Count().Should().Be(originalCount-1);
        _context.Users.Include(x => x.Friends)
            .First(x => x.Id == "fake-user-5").Friends.Should().NotBeEmpty();
        _context.Users.Include(x => x.Friends)
            .First(x => x.Id == "fake-user-6").Friends.Should().NotBeEmpty();
    }
    
    [Test]
    public async Task AcceptFriendRequest_should_throw_exception_when_request_is_not_found()
    {
        var originalCount = _context.FriendRequests.Count();
        var input = new FriendRequestInput
        {
            UserId = "fake-user-66",
            FriendId = "fake-user-6"
        };

        var act = () => _sut.AcceptFriendRequest(input);

        await act.Should().ThrowAsync<Exception>().WithMessage("Friend request not found");
    }
    
    [Test]
    public async Task RemoveFriendRequest_should_remove_request_when_requester_in_userId()
    {
        var originalCount = _context.FriendRequests.Count();

        var act = () => _sut.RemoveFriendRequest("fake-request-1", "fake-user-5");

        await act.Should().NotThrowAsync();
        _context.FriendRequests.Count().Should().Be(originalCount-1);
    }
    
    [Test]
    public async Task RemoveFriendRequest_should_remove_request_when_friend_in_userId()
    {
        var originalCount = _context.FriendRequests.Count();

        var act = () => _sut.RemoveFriendRequest("fake-request-1", "fake-user-6");

        await act.Should().NotThrowAsync();
        _context.FriendRequests.Count().Should().Be(originalCount-1);
    }
}