using GamifiedToDo.Services.App.Int.Users;

namespace GamifiedToDo.Services.App.Dep.FriendRequests;

public interface IFriendRequestRepository
{
    Task CreateFriendRequest(FriendRequestInput input, CancellationToken cancellationToken = default);
    Task AcceptFriendRequest(FriendRequestInput input, CancellationToken cancellationToken = default);
}