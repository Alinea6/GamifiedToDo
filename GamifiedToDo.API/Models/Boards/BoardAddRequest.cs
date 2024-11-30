namespace GamifiedToDo.API.Models.Boards;

public class BoardAddRequest
{
    public string Name { get; set; }

    public IEnumerable<string> Collaborators { get; set; } = new List<string>();
    
    public IEnumerable<string> ChoreIds { get; set; } = new List<string>();
}