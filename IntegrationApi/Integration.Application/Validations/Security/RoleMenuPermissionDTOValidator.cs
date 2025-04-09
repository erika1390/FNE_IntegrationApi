using FluentValidation;
using Integration.Shared.DTO.Security;

namespace Integration.Application.Validations.Security
{
    public class RoleMenuPermissionDTOValidator : AbstractValidator<RoleMenuPermissionDTO>
    {
        public RoleMenuPermissionDTOValidator()
        {
            RuleFor(x => x.RoleCode)
                .NotEmpty().WithMessage("El código del rol es obligatorio.")
                .MaximumLength(10).WithMessage("El código del rol no puede exceder los 10 caracteres.");

            RuleFor(x => x.MenuCode)
                .NotEmpty().WithMessage("El código del módulo es obligatorio.")
                .MaximumLength(10).WithMessage("El código del módulo no puede exceder los 10 caracteres.");

            RuleFor(x => x.PermissionCode)
                .NotEmpty().WithMessage("El código del permiso es obligatorio.")
                .MaximumLength(10).WithMessage("El código del permiso no puede exceder los 10 caracteres.");

            RuleFor(x => x.CreatedAt)
                .NotEmpty().WithMessage("La fecha de creación es obligatoria.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La fecha de creación no puede ser en el futuro.");

            RuleFor(x => x.UpdatedAt)
                .GreaterThanOrEqualTo(x => x.CreatedAt)
                .When(x => x.UpdatedAt.HasValue)
                .WithMessage("La fecha de actualización no puede ser menor a la fecha de creación.");

            RuleFor(x => x.CreatedBy)
                .NotEmpty().WithMessage("El usuario que creó el registro es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre del usuario creador no puede exceder los 50 caracteres.");

            RuleFor(x => x.UpdatedBy)
                .MaximumLength(50).WithMessage("El nombre del usuario actualizador no puede exceder los 50 caracteres.");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("El estado de la relación es obligatorio.");
        }
    }
}