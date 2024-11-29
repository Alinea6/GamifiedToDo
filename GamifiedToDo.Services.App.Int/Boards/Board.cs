namespace GamifiedToDo.Services.App.Int.Boards;

public class Board
{
    public string Id { get; set; }
    
    public string UserId { get; set; }
    
    public IEnumerable<string> Collaborators { get; set; }
    
    public string Name { get; set; }
    
    public IEnumerable<string> ChoreIds { get; set; } = new List<string>();
}