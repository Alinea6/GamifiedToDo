namespace GamifiedToDo.Services.App.Int.Chores;

public class ChoreUpdateInput
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string? ChoreText { get; set; }
    public ChoreDifficulty? Difficulty { get; set; }
    public ChoreCategory? Category { get; set; }
}