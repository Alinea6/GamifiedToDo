using FluentValidation;
using GamifiedToDo.API.Models.Boards;

namespace GamifiedToDo.API.Validators;

public class BoardChoresRequestValidator : AbstractValidator<BoardChoresRequest>
{
    public BoardChoresRequestValidator()
    {
        RuleFor(x => x.ChoreIds).NotEmpty();
    }
}