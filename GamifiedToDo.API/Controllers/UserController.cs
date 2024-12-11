using GamifiedToDo.API.Models;
using GamifiedToDo.API.Models.Users;
using GamifiedToDo.Services.App.Int.Users;
using Microsoft.AspNetCore.Mvc;

namespace GamifiedToDo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public Task<string> Register(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        return _userService.Register(new RegisterInput
        {
            Login = request.Login,
            Password = request.Password,
            Email = request.Email
        }, cancellationToken);
    }
    
    [HttpPost("login")]
    public Task<string> Login(LoginRequest request, CancellationToken cancellationToken = default)
    {
        return _userService.Login(new LoginInput
        {
            Login = request.Login,
            Password = request.Password
        }, cancellationToken);
    }

    [HttpGet]
    public Task<IEnumerable<User>> GetUsers(
        [FromQuery] string? search = null, 
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default)
    {
        return _userService.GetUsers(new GetUsersInput
        {
            Search = search,
            PageNumber = pageNumber,
            PageSize = pageSize,
        }, cancellationToken);
    }

    [HttpPost("friends/request")]
    public Task CreateFriendRequest(FriendRequest request, CancellationToken cancellationToken = default)
    {
        return _userService.CreateFriendRequest(
            new FriendRequestInput
            {
                UserId = GetUserId(),
                FriendId = request.FriendId
            }, cancellationToken);
    }

    [HttpGet("friends/{requesterId}/accept")]
    public Task AcceptFriendRequest(string requesterId, CancellationToken cancellationToken = default)
    {
        return _userService.AcceptFriendRequest(new FriendRequestInput
        {
            UserId = requesterId,
            FriendId = GetUserId()
        }, cancellationToken);
    }
}