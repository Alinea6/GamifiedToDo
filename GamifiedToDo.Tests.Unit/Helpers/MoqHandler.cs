using FluentAssertions;
using Moq;

namespace GamifiedToDo.Tests.Unit.Helpers;

public class MoqHandler
{
    public static TValue IsEquivalentTo<TValue>(TValue input)
    {
        return It.Is<TValue>(x => Compare(input, x));
    }

    private static bool Compare<TValue>(TValue input, TValue realCall)
    {
        input.Should().BeEquivalentTo(realCall);
        return true;
    }
}