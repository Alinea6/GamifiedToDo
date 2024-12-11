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
}