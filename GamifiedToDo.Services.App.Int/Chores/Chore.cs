using GamifiedToDo.Services.App.Int.Chores;

namespace GamifiedToDo.Services.App.Int;

public class Chore
{
    public string Id { get; set; }
    
    public string UserId { get; set; }
    
    public string ChoreText { get; set; }
    
    public ChoreStatus Status { get; set; }
    
    public ChoreDifficulty Difficulty { get; set; }
    
    public ChoreCategory Category { get; set; }
}