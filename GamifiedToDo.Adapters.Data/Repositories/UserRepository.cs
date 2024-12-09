using System.IdentityModel.Tokens.Jwt;
using GamifiedToDo.Adapters.Data.Models;
using GamifiedToDo.Services.App.Dep.Users;
using GamifiedToDo.Services.App.Int.Users;
using Microsoft.EntityFrameworkCore;
using User = GamifiedToDo.Services.App.Int.Users.User;

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
        var userId = Guid.NewGuid().ToString();
        var user = new Models.User
        {
            Id = userId,
            Login = input.Login,
            Password = input.Password,
            Email = input.Email,
            UserLevel = new UserLevel
            {
                Id = userId,
                Exp = 0
            }
        };

        _dataContext.Add(user);
        await _dataContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        return user.Id;
    }
    
    public async Task<string> Login(LoginInput input, CancellationToken cancellationToken = default)
    {
        var user = await _dataContext.Users
            .Where(x => x.Login == input.Login && x.Password == input.Password)
            .SingleOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new Exception("Incorrect login or password");
        }

        JwtSecurityToken jwtSecurityToken = _jwtProvider.GenerateToken(user.Id);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    public async Task<IEnumerable<User>> GetUsers(string? search = null, CancellationToken cancellationToken = default)
    {
        IQueryable<Models.User> query = _dataContext.Users;

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(x => x.Login.ToLower().Contains(search.ToLower()));
        }

        var users = await query
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        return users.Select(MapToUser);
    }
    
    private static User MapToUser(Models.User user)
    {
        return new User
        {
            Id = user.Id,
            Login = user.Login
        };
    }
}