using FluentValidation;
using Integration.Shared.DTO.Security;
namespace Integration.Application.Validations.Security
{
    public class RoleDTOValidator : AbstractValidator<RoleDTO>
    {
        public RoleDTOValidator()
        {
            RuleFor(role => role.Code)
                .NotEmpty().WithMessage("El código del rol es requerido.")
                .MaximumLength(10).WithMessage("El código del rol no puede exceder los 50 caracteres.");

            RuleFor(role => role.Name)
                .NotEmpty().WithMessage("El nombre del rol es requerido.")
                .MaximumLength(50).WithMessage("El nombre del rol no puede exceder los 100 caracteres.");

            RuleFor(role => role.ApplicationCode)
                .MaximumLength(10).WithMessage("El código de la aplicación no puede exceder los 50 caracteres.")
                .When(role => !string.IsNullOrEmpty(role.ApplicationCode));

            RuleFor(role => role.CreatedAt)
                .NotEmpty().WithMessage("La fecha de creación es requerida.");

            RuleFor(role => role.CreatedBy)
                .NotEmpty().WithMessage("El usuario que creó el rol es requerido.")
                .MaximumLength(50).WithMessage("El usuario creador no puede exceder los 50 caracteres.");

            RuleFor(role => role.UpdatedBy)
                .MaximumLength(50).WithMessage("El usuario que actualizó el rol no puede exceder los 50 caracteres.")
                .When(role => !string.IsNullOrEmpty(role.UpdatedBy));

            RuleFor(role => role.Application)
                .SetValidator(new ApplicationDTOValidator())
                .When(role => role.Application is not null);
        }
    }
}
