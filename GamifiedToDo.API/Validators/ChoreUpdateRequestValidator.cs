using FluentValidation;
using GamifiedToDo.API.Models;

namespace GamifiedToDo.API.Validators;

public class ChoreUpdateRequestValidator : AbstractValidator<ChoreUpdateRequest>
{
    public ChoreUpdateRequestValidator()
    {
        RuleFor(x => x.ChoreText).NotEmpty();
    }
}