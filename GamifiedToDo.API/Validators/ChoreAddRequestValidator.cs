using FluentValidation;
using GamifiedToDo.API.Models;

namespace GamifiedToDo.API.Validators;

public class ChoreAddRequestValidator : AbstractValidator<ChoreAddRequest>
{
    public ChoreAddRequestValidator()
    {
        RuleFor(x => x.ChoreText).NotEmpty();
    }
}