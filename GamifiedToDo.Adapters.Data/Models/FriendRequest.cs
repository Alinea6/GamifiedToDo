using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamifiedToDo.Adapters.Data.Models;

[Table("FriendRequest")]
public class FriendRequest
{
    [Key]
    public string Id { get; set; }
    
    public string UserId { get; set; }
    
    public string FriendId { get; set; }
    
    public User User { get; set; }
    
    public User Friend { get; set; }
}