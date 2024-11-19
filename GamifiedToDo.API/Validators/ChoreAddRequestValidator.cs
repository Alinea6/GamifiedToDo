using FluentValidation;
using GamifiedToDo.API.Models;
using GamifiedToDo.API.Models.Chores;

namespace GamifiedToDo.API.Validators;

public class ChoreAddRequestValidator : AbstractValidator<ChoreAddRequest>
{
    public ChoreAddRequestValidator()
    {
        RuleFor(x => x.ChoreText).NotEmpty();
    }
}