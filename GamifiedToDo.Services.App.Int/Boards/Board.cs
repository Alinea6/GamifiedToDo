using GamifiedToDo.Services.App.Int.Chores;
using GamifiedToDo.Services.App.Int.Users;

namespace GamifiedToDo.Services.App.Int.Boards;

public class Board
{
    public string Id { get; set; }
    
    public User Owner { get; set; }
    
    public IEnumerable<User> Collaborators { get; set; }
    
    public string Name { get; set; }
    
    public IEnumerable<Chore> Chores { get; set; }
    
    public bool IsOwner { get; set; }
}