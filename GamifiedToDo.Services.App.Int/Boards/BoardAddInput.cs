namespace GamifiedToDo.Services.App.Int.Boards;

public class BoardAddInput
{
    public string UserId { get; set; }
    
    public string Name { get; set; }

    public IEnumerable<string> Collaborators { get; set; } = new List<string>();
    
    public IEnumerable<string> ChoreIds { get; set; } = new List<string>();
}