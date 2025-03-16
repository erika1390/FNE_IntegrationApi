using FluentValidation;
using Integration.Shared.DTO.Security;
namespace Integration.Application.Validations.Security
{
    public class ApplicationDTOValidator : AbstractValidator<ApplicationDTO>
    {
        public ApplicationDTOValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("El código de la aplicación es obligatorio.")
                .MaximumLength(20).WithMessage("El código no puede exceder los 20 caracteres.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la aplicación es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

            RuleFor(x => x.CreatedAt)
                .NotEmpty().WithMessage("La fecha de creación es obligatoria.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La fecha de creación no puede ser en el futuro.");

            RuleFor(x => x.UpdatedAt)
                .GreaterThanOrEqualTo(x => x.CreatedAt)
                .When(x => x.UpdatedAt.HasValue)
                .WithMessage("La fecha de actualización no puede ser menor a la fecha de creación.");

            RuleFor(x => x.CreatedBy)
                .NotEmpty().WithMessage("El usuario que creó la aplicación es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre del usuario no puede exceder los 50 caracteres.");

            RuleFor(x => x.UpdatedBy)
                .NotEmpty().WithMessage("El usuario que actualizó la aplicación es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre del usuario no puede exceder los 50 caracteres.");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("El estado de la aplicación es obligatorio.");
        }
    }
}