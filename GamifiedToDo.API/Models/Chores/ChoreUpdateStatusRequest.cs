using GamifiedToDo.Services.App.Int;

namespace GamifiedToDo.API.Models.Chores;

public class ChoreUpdateStatusRequest
{
    public ChoreStatus Status { get; set; }
}