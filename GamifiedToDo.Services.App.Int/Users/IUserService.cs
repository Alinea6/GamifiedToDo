namespace GamifiedToDo.Services.App.Int.Users;

public interface IUserService
{
    Task<string> Register(RegisterInput input, CancellationToken cancellationToken = default);
    Task<string> Login(LoginInput input, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetUsers(string? search = null, CancellationToken cancellationToken = default);
}