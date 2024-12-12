namespace GamifiedToDo.Services.App.Int.Users;

public interface IUserService
{
    Task<string> Register(RegisterInput input, CancellationToken cancellationToken = default);
    Task<string> Login(LoginInput input, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetUsers(GetUsersInput input, CancellationToken cancellationToken = default);
    Task CreateFriendRequest(FriendRequestInput input, CancellationToken cancellationToken = default);
    Task AcceptFriendRequest(FriendRequestInput input, CancellationToken cancellationToken = default);
    Task RemoveFriendRequest(string requestId, string userId, CancellationToken cancellationToken = default);
    Task RemoveFriend(string userId, string friendId, CancellationToken cancellationToken = default);
    Task<UserFriends> GetUserFriends(string userId, CancellationToken cancellationToken = default);
}