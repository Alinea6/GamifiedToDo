namespace GamifiedToDo.Services.App.Int.Chores;

public class ChoreAddInput
{
    public string UserId { get; set; }
    public string ChoreText { get; set; }
    public ChoreStatus Status { get; set; }
    public ChoreDifficulty Difficulty { get; set; }
    public ChoreCategory Category { get; set; }
    public string? BoardId { get; set; }
}