using GamifiedToDo.Services.App.Int;

namespace GamifiedToDo.API.Models;

public class ChoreUpdateRequest
{
    public string? ChoreText { get; set; }
    public ChoreStatus? Status { get; set; }
}