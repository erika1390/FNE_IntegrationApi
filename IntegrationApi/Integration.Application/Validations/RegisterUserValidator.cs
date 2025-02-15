using FluentValidation;

using Integration.Shared.DTO.Aut;

namespace Integration.Application.Validations
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDTO>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Correo inválido");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres");
            RuleFor(x => x.ConfirmPassword).NotEmpty().MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres");
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50).WithMessage("Nombre inválido");
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50).WithMessage("Apellido inválido");
        }
    }
}