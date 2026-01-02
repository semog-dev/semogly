using FluentValidation;
using Semogly.Core.Domain.AccountContext.Validations;

namespace Semogly.Core.Application.AccountContext.UseCases.Create;

public sealed class Validator : AbstractValidator<Command>
{
    public Validator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("O nome é obrigatório.");
        RuleFor(x => x.FirstName).MinimumLength(3).WithMessage("O nome deve conter pelo menos 3 caracteres.");
        RuleFor(x => x.FirstName).MaximumLength(40).WithMessage("O nome deve conter no máximo 40 caracteres.");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("O sobrenome é obrigatório.");
        RuleFor(x => x.LastName).MinimumLength(3).WithMessage("O sobrenome deve conter pelo menos 3 caracteres.");
        RuleFor(x => x.LastName).MaximumLength(40).WithMessage("O sobrenome deve conter no máximo 40 caracteres.");

        RuleFor(x => x.Email).NotEmpty().WithMessage("O E-mail é obrigatório.");
        RuleFor(x => x.Email).EmailAddress().WithMessage("O E-mail informado é inválido.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("A senha é obrigatória.");
        RuleFor(x => x.Password).Matches(Regexes.PasswordRegex()).WithMessage("A senha deve conter pelo menos 8 caracteres 1 letra maiúscula 1 número e 1 caracter especial.");
    }
}