using FluentAssertions;
using GamifiedToDo.Adapters.Data;
using GamifiedToDo.Adapters.Data.Repositories;
using GamifiedToDo.Services.App.Int.Users;
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
            UserId = "fake-user-5",
            FriendId = "fake-user-6"
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
}