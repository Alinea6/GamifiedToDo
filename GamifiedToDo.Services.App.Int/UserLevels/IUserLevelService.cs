namespace GamifiedToDo.Services.App.Int.UserLevels;

public interface IUserLevelService
{
    Task<int> UpdateExp(string userId, int choreDifficulty, CancellationToken cancellationToken = default);
}