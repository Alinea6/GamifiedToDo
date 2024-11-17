using GamifiedToDo.Services.App.Dep.Users;
using GamifiedToDo.Services.App.Int.Users;

namespace GamifiedToDo.Services.App.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<string> Register(RegisterInput input, CancellationToken cancellationToken = default)
    {
        return _userRepository.Register(input, cancellationToken);
    }
}