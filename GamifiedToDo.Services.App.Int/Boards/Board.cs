using GamifiedToDo.Services.App.Int.Chores;
using GamifiedToDo.Services.App.Int.Users;

namespace GamifiedToDo.Services.App.Int.Boards;

public class Board
{
    public string Id { get; set; }
    
    public string UserId { get; set; }
    
    public IEnumerable<User> Collaborators { get; set; }
    
    public string Name { get; set; }
    
    public IEnumerable<Chore> Chores { get; set; }
}