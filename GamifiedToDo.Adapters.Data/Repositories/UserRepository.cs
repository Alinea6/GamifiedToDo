using GamifiedToDo.Services.App.Dep.Users;
using GamifiedToDo.Services.App.Int.Users;

namespace GamifiedToDo.Adapters.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;
    private readonly IJsonWebTokenProvider _jwtProvider;

    public UserRepository(DataContext dataContext, IJsonWebTokenProvider jwtProvider)
    {
        _dataContext = dataContext;
        _jwtProvider = jwtProvider;
    }

    public async Task<string> Register(RegisterInput input, CancellationToken cancellationToken = default)
    {
        var user = new Models.User
        {
            Id = Guid.NewGuid().ToString(),
            Login = input.Login,
            Password = input.Password,
            Email = input.Email
        };

        _dataContext.Add(user);
        await _dataContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        return user.Id;
    }
}