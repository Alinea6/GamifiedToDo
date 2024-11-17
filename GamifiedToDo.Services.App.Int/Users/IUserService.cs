namespace GamifiedToDo.Services.App.Int.Users;

public interface IUserService
{
    Task<string> Register(RegisterInput input, CancellationToken cancellationToken = default);
}