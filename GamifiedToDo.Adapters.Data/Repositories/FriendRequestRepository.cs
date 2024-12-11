using GamifiedToDo.Services.App.Dep.FriendRequests;
using GamifiedToDo.Services.App.Int.Users;
using Microsoft.EntityFrameworkCore;

namespace GamifiedToDo.Adapters.Data.Repositories;

public class FriendRequestRepository : IFriendRequestRepository
{
    private readonly DataContext _dataContext;

    public FriendRequestRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }


    public async Task CreateFriendRequest(FriendRequestInput input, CancellationToken cancellationToken = default)
    {
        var users = await _dataContext.Users
            .Where(user => user.Id == input.UserId || user.Id == input.FriendId)
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        var requester = users.First(x => x.Id == input.UserId);
        var friend = users.First(x => x.Id == input.FriendId);

        var request = new Models.FriendRequest
        {
            Id = Guid.NewGuid().ToString(),
            UserId = input.UserId,
            User = requester,
            FriendId = input.FriendId,
            Friend = friend
        };

        _dataContext.Add(request);
        await _dataContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task AcceptFriendRequest(FriendRequestInput input, CancellationToken cancellationToken = default)
    {
        var request = await _dataContext.FriendRequests
            .Where(x => x.UserId == input.UserId && x.FriendId == input.FriendId)
            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        if (request == null)
        {
            throw new Exception("Friend request not found");
        }
        
        var user = request.User;
        var friend = request.Friend;

        _dataContext.Remove(request);
        
        user.Friends.Add(friend);
        friend.Friends.Add(user);
        
        await _dataContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}