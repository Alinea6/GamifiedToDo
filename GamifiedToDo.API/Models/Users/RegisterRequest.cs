namespace GamifiedToDo.API.Models.Users;

public class RegisterRequest
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}