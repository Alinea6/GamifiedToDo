using GamifiedToDo.Services.App.Int;

namespace GamifiedToDo.API.Models.Chores;

public class ChoreUpdateRequest
{
    public string? ChoreText { get; set; }
    public ChoreStatus? Status { get; set; }
}