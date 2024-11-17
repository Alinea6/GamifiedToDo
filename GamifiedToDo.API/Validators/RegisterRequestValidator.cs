using FluentValidation;
using GamifiedToDo.API.Models;

namespace GamifiedToDo.API.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Login).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().MaximumLength(50).EmailAddress();
    }
}