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
    
    public IEnumerable<string> Collaborators { get; set; } = new List<string>();
    
    [Required]
    public string Name { get; set; }

    public IEnumerable<string> ChoreIds { get; set; } = new List<string>();

}