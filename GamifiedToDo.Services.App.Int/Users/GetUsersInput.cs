namespace GamifiedToDo.Services.App.Int.Users;

public class GetUsersInput
{
    public string? Search { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}