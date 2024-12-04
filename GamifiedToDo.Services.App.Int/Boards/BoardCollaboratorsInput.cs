namespace GamifiedToDo.Services.App.Int.Boards;

public class BoardCollaboratorsInput
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public IEnumerable<string> CollaboratorIds { get; set; }
}