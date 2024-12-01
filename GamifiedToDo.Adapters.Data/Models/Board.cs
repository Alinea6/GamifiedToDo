using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamifiedToDo.Adapters.Data.Models;

[Table("Board")]
public class Board
{
    [Key]
    public string Id { get; set; }
    
    [Required]
    public string UserId { get; set; }

    public ICollection<User> Collaborators { get; set; } = [];

    [Required]
    public string Name { get; set; }

    public ICollection<Chore> Chores { get; set; } = [];

    public User Owner { get; set; }
}