using FluentValidation.TestHelper;

using Integration.Application.Validations.Security;
using Integration.Shared.DTO.Security;

namespace Integration.Application.Test.Validations.Security
{
    [TestFixture]
    public class RoleDTOValidatorTest
    {
        private RoleDTOValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new RoleDTOValidator();
        }

        [Test]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var model = new RoleDTO { Code = "ROL0000001", Name = "", CreatedAt = DateTime.UtcNow, CreatedBy = "System" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("El nombre del rol es requerido.");
        }

        [Test]
        public void Should_Have_Error_When_Name_Exceeds_MaxLength()
        {
            var model = new RoleDTO { Code = "ROL0000001", Name = new string('A', 51), CreatedAt = DateTime.UtcNow, CreatedBy = "System" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("El nombre del rol no puede exceder los 100 caracteres.");
        }

        [Test]
        public void Should_Have_Error_When_CreatedAt_Is_Missing()
        {
            var model = new RoleDTO { Code = "ROL0000001", Name = "Administrador", CreatedBy = "System" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedAt)
                .WithErrorMessage("La fecha de creación es requerida.");
        }

        [Test]
        public void Should_Have_Error_When_CreatedBy_Is_Empty()
        {
            var model = new RoleDTO { Code = "ROL0000001", Name = "Administrador", CreatedAt = DateTime.UtcNow, CreatedBy = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedBy)
                .WithErrorMessage("El usuario que creó el rol es requerido.");
        }

        [Test]
        public void Should_Have_Error_When_CreatedBy_Exceeds_MaxLength()
        {
            var model = new RoleDTO { Code = "ROL0000001", Name = "Administrador", CreatedAt = DateTime.UtcNow, CreatedBy = new string('A', 51) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedBy)
                .WithErrorMessage("El usuario creador no puede exceder los 50 caracteres.");
        }

        [Test]
        public void Should_Have_Error_When_UpdatedBy_Exceeds_MaxLength()
        {
            var model = new RoleDTO
            {
                Code = "ROL0000001",
                Name = "Administrador",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System",
                UpdatedBy = new string('B', 51)
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UpdatedBy)
                .WithErrorMessage("El usuario que actualizó el rol no puede exceder los 50 caracteres.");
        }

        [Test]
        public void Should_Not_Have_Error_When_Application_Is_Valid()
        {
            var now = DateTime.UtcNow.AddSeconds(-1); // 🔁 Restamos 1 segundo

            var model = new RoleDTO
            {
                Code = "ROL0000001",
                Name = "Administrador",
                CreatedAt = now,
                CreatedBy = "System",
                UpdatedBy = "System",
                IsActive = true,
                Application = new ApplicationDTO
                {
                    Code = "APP0000001",
                    Name = "App",
                    CreatedAt = now,
                    UpdatedAt = now.AddMinutes(1),
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsActive = true
                }
            };

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}