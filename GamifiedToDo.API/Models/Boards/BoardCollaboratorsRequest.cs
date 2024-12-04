namespace GamifiedToDo.API.Models.Boards;

public class BoardCollaboratorsRequest
{
    public IEnumerable<string> CollaboratorIds { get; set; }
}