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

    public string[] Collaborators { get; set; } = [];

    [Required]
    public string Name { get; set; }

    public string[] ChoreIds { get; set; } = [];

}