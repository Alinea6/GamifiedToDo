namespace GamifiedToDo.Services.App.Dep.UserLevels;

public interface IUserLevelRepository
{
    Task<int> UpdateExp(string userId, int experienceToAdd, CancellationToken cancellationToken = default);
}