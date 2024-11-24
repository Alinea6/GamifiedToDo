using GamifiedToDo.Services.App.Int.Chores;

namespace GamifiedToDo.API.Models.Chores;

public class ChoreAddRequest
{
    public string ChoreText { get; set; }
    public ChoreDifficulty Difficulty { get; set; }
    public ChoreCategory Category { get; set; }
}