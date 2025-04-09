using FluentValidation;

using Integration.Shared.DTO.Security;

namespace Integration.Application.Validations.Security
{
    public class MenuDTOValidator : AbstractValidator<MenuDTO>
    {
        public MenuDTOValidator()
        {
            RuleFor(x => x.ModuleCode)
                .NotEmpty().WithMessage("El codigo del nodulo es obligatorio.")
                .MaximumLength(10).WithMessage("El codigo del menu no puede exceder los 10 caracteres.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del menu es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

            RuleFor(x => x.Order)
                .NotNull().WithMessage("El orden del menu es obligatorio.");

            RuleFor(x => x.CreatedAt)
                .NotEmpty().WithMessage("La fecha de creación es obligatoria.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La fecha de creación no puede ser en el futuro.");

            RuleFor(x => x.UpdatedAt)
                .GreaterThanOrEqualTo(x => x.CreatedAt)
                .When(x => x.UpdatedAt.HasValue)
                .WithMessage("La fecha de actualización no puede ser menor a la fecha de creación.");

            RuleFor(x => x.CreatedBy)
                .NotEmpty().WithMessage("El usuario que creó el módulo es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre del usuario no puede exceder los 50 caracteres.");

            RuleFor(x => x.UpdatedBy)
                .MaximumLength(50).WithMessage("El nombre del usuario no puede exceder los 50 caracteres.");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("El estado del módulo es obligatorio.");
        }
    }
}