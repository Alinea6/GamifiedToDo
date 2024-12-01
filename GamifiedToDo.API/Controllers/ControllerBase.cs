namespace GamifiedToDo.API.Controllers;

public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    protected string GetUserId()
    {
        var userId = User.Claims.FirstOrDefault()?.Value;

        if (userId == null)
        {
            throw new Exception("User id was not found");
        }
        
        return userId;
    }
}