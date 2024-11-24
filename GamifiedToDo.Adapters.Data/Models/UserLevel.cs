using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamifiedToDo.Adapters.Data.Models;

[Table("UserLevel")]
public class UserLevel
{
    [Key]
    public string Id { get; set; }
    public int Exp { get; set; }
    public User User { get; set; }
}