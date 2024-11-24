namespace GamifiedToDo.Services.App.Int.Chores;

public class ChoreUpdateStatusInput
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public ChoreStatus Status { get; set; }
}