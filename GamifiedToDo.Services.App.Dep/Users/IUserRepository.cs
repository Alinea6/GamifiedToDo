using GamifiedToDo.Services.App.Int.Users;

namespace GamifiedToDo.Services.App.Dep.Users;

public interface IUserRepository
{
    Task<string> Register(RegisterInput input, CancellationToken cancellationToken = default);
    Task<string> Login(LoginInput input, CancellationToken cancellationToken = default); 
}