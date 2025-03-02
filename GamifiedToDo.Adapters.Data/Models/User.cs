using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamifiedToDo.Adapters.Data.Models;

[Table("User")]
public class User
{
    [Key]
    public string Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Login { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Password { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Email { get; set; }
    
    public UserLevel UserLevel { get; set; }

    public ICollection<Board> CollaborationBoards { get; set; } = [];
    
    public ICollection<User> Friends { get; set; } = [];
}