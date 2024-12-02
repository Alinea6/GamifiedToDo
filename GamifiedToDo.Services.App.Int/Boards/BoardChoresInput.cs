namespace GamifiedToDo.Services.App.Int.Boards;

public class BoardChoresInput
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public IEnumerable<string> ChoreIds { get; set; }
}