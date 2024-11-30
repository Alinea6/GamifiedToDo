using FluentValidation;
using GamifiedToDo.API.Models.Boards;

namespace GamifiedToDo.API.Validators;

public class BoardAddRequestValidator : AbstractValidator<BoardAddRequest>
{
    public BoardAddRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}