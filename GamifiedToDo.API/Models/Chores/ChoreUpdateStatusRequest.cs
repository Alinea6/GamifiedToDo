using GamifiedToDo.Services.App.Int;
using GamifiedToDo.Services.App.Int.Chores;

namespace GamifiedToDo.API.Models.Chores;

public class ChoreUpdateStatusRequest
{
    public ChoreStatus Status { get; set; }
}