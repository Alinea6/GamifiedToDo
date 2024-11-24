using FluentAssertions;
using GamifiedToDo.Adapters.Data;
using GamifiedToDo.Adapters.Data.Repositories;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Unit.Adapters;

public class UserLevelRepositoryTest
{
    private UserLevelRepository _sut;
    private DataContext _context;

    [SetUp]
    public void SetUp()
    {
        _context = FakeDbContextProvider.GetFakeDbContext();
        _sut = new UserLevelRepository(_context);
    }

    [Test]
    public async Task UpdateExp_should_find_user_level_and_update_exp()
    {
        // act
        var result = await _sut.UpdateExp("fake-user-5", 20);

        result.Should().Be(21);
    }

    [Test]
    public async Task UpdateExp_should_not_found_user_level_and_throw_exception()
    {
        var act = () => _sut.UpdateExp("fake-user-99", 100);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("User level with userId fake-user-99 does not exist");
    }
}