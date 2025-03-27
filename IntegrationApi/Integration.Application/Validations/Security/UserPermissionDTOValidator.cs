using FluentValidation;

using Integration.Shared.DTO.Security;

namespace Integration.Application.Validations.Security
{
    public class UserPermissionDTOValidator : AbstractValidator<UserPermissionDTORequest>
    { 
        public UserPermissionDTOValidator() 
        {
            RuleFor(x => x.UserCode)
                .NotEmpty().WithMessage("El código del usuario es obligatorio.")
                .MaximumLength(10).WithMessage("El código del usuario no puede exceder los 10 caracteres.");
        }
    }
}