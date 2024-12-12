namespace GamifiedToDo.Services.App.Int.Users;

public class UserFriends
{
    public string UserId { get; set; }
    public IEnumerable<User> Friends { get; set; }
}