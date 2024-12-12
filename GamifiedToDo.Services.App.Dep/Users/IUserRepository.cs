using GamifiedToDo.Services.App.Int.Users;

namespace GamifiedToDo.Services.App.Dep.Users;

public interface IUserRepository
{
    Task<string> Register(RegisterInput input, CancellationToken cancellationToken = default);
    Task<string> Login(LoginInput input, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetUsers(GetUsersInput input, CancellationToken cancellationToken = default);
    Task RemoveFriend(string userId, string friendId, CancellationToken cancellationToken = default);
    Task<UserFriends> GetUserFriends(string userId, CancellationToken cancellationToken = default);
}