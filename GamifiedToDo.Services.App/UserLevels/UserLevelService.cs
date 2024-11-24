using GamifiedToDo.Services.App.Dep.UserLevels;
using GamifiedToDo.Services.App.Int.UserLevels;

namespace GamifiedToDo.Services.App.UserLevels;

public class UserLevelService : IUserLevelService
{
    private const int ExpMultiplier = 5;
    private readonly IUserLevelRepository _userLevelRepository;

    public UserLevelService(IUserLevelRepository userLevelRepository)
    {
        _userLevelRepository = userLevelRepository;
    }

    public async Task<int> UpdateExp(string userId, int choreDifficulty, CancellationToken cancellationToken = default)
    {
        var experienceToAdd = choreDifficulty * ExpMultiplier;
        return await _userLevelRepository.UpdateExp(userId, experienceToAdd, cancellationToken).ConfigureAwait(false);
    }
}