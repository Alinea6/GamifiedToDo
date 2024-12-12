using GamifiedToDo.Services.App.Dep.FriendRequests;
using GamifiedToDo.Services.App.Dep.Users;
using GamifiedToDo.Services.App.Int.Users;

namespace GamifiedToDo.Services.App.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IFriendRequestRepository _friendRequestRepository;

    public UserService(
        IUserRepository userRepository,
        IFriendRequestRepository friendRequestRepository)
    {
        _userRepository = userRepository;
        _friendRequestRepository = friendRequestRepository;
    }

    public Task<string> Register(RegisterInput input, CancellationToken cancellationToken = default)
    {
        return _userRepository.Register(input, cancellationToken);
    }
    
    public Task<string> Login(LoginInput input, CancellationToken cancellationToken = default)
    {
        return _userRepository.Login(input, cancellationToken);
    }

    public Task<IEnumerable<User>> GetUsers(GetUsersInput input, CancellationToken cancellationToken = default)
    {
        return _userRepository.GetUsers(input, cancellationToken);
    }

    public Task CreateFriendRequest(FriendRequestInput input, CancellationToken cancellationToken = default)
    {
        return _friendRequestRepository.CreateFriendRequest(input, cancellationToken);
    }

    public Task AcceptFriendRequest(FriendRequestInput input, CancellationToken cancellationToken = default)
    {
        return _friendRequestRepository.AcceptFriendRequest(input, cancellationToken);
    }

    public Task RemoveFriendRequest(string requestId, string userId, CancellationToken cancellationToken = default)
    {
        return _friendRequestRepository.RemoveFriendRequest(requestId, userId, cancellationToken);
    }

    public Task RemoveFriend(string userId, string friendId, CancellationToken cancellationToken = default)
    {
        return _userRepository.RemoveFriend(userId, friendId, cancellationToken);
    }

    public Task<UserFriends> GetUserFriends(string userId, CancellationToken cancellationToken = default)
    {
        return _userRepository.GetUserFriends(userId, cancellationToken);
    }
}