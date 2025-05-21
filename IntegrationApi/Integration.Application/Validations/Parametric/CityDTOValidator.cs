using FluentValidation;
using Integration.Shared.DTO.Parametric;

namespace Integration.Application.Validations.Parametric
{
    public class CityDTOValidator : AbstractValidator<CityDTO>
    {
        public CityDTOValidator()
        {
            RuleFor(x => x.DepartmentId)
               .NotNull().WithMessage("El id del departamento es obligatorio.");

            RuleFor(x => x.CodeDane)
                .NotEmpty().WithMessage("El codigo dane es obligatorio.")
                .MaximumLength(5).WithMessage("La abreviatura no puede exceder los 5 caracteres.");

            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("El nombre es obligatorio.")
               .MaximumLength(50).WithMessage("El nombre no puede exceder los 50 caracteres.");

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