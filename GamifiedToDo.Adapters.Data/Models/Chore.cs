using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamifiedToDo.Adapters.Data.Models;

[Table("Chore")]
public class Chore
{
    [Key]
    public string Id { get; set; }
    
    [Required]
    public string UserId { get; set; }
    
    public string ChoreText { get; set; }
    
    public string Status { get; set; }
}