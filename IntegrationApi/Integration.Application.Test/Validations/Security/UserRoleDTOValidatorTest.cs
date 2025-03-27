using FluentValidation.TestHelper;

using Integration.Application.Validations.Security;
using Integration.Shared.DTO.Security;

namespace Integration.Application.Test.Validations.Security
{
    [TestFixture]
    public class UserRoleDTOValidatorTest
    {
        private UserRoleDTOValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new UserRoleDTOValidator();
        }

        [Test]
        public void Should_Have_Error_When_UserCode_Is_Empty()
        {
            var model = new UserRoleDTO { UserCode = "", RoleCode = "ROL0000001", CreatedAt = DateTime.UtcNow, CreatedBy = "admin", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UserCode)
                .WithErrorMessage("El código del usuario es obligatorio.");
        }

        [Test]
        public void Should_Have_Error_When_UserCode_Exceeds_MaxLength()
        {
            var model = new UserRoleDTO { UserCode = new string('U', 11), RoleCode = "ROL0000001", CreatedAt = DateTime.UtcNow, CreatedBy = "admin", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UserCode)
                .WithErrorMessage("El código del usuario no puede exceder los 10 caracteres.");
        }

        [Test]
        public void Should_Have_Error_When_RoleCode_Is_Empty()
        {
            var model = new UserRoleDTO { UserCode = "USR0000001", RoleCode = "", CreatedAt = DateTime.UtcNow, CreatedBy = "admin", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.RoleCode)
                .WithErrorMessage("El código del rol es obligatorio.");
        }

        [Test]
        public void Should_Have_Error_When_RoleCode_Exceeds_MaxLength()
        {
            var model = new UserRoleDTO
            {
                UserCode = "USR0000001",
                RoleCode = new string('R', 11), // Excede los 10 caracteres permitidos
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "admin",
                IsActive = true
            };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.RoleCode)
                .WithErrorMessage("El código del rol no puede exceder los 10 caracteres."); // ✅ Mensaje correcto según el validador
        }

        [Test]
        public void Should_Have_Error_When_CreatedAt_Is_In_Future()
        {
            var model = new UserRoleDTO { UserCode = "USR0000001", RoleCode = "ROL0000001", CreatedAt = DateTime.UtcNow.AddMinutes(1), CreatedBy = "admin", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedAt)
                .WithErrorMessage("La fecha de creación no puede ser en el futuro.");
        }

        [Test]
        public void Should_Have_Error_When_UpdatedAt_Is_Before_CreatedAt()
        {
            var now = DateTime.UtcNow;
            var model = new UserRoleDTO
            {
                UserCode = "USR0000001",
                RoleCode = "ROL0000001",
                CreatedAt = now,
                UpdatedAt = now.AddMinutes(-5),
                CreatedBy = "admin",
                IsActive = true
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UpdatedAt)
                .WithErrorMessage("La fecha de actualización no puede ser menor a la fecha de creación.");
        }

        [Test]
        public void Should_Have_Error_When_CreatedBy_Is_Empty()
        {
            var model = new UserRoleDTO { UserCode = "USR0000001", RoleCode = "ROL0000001", CreatedAt = DateTime.UtcNow, CreatedBy = "", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedBy)
                .WithErrorMessage("El usuario que creó el registro es obligatorio.");
        }

        [Test]
        public void Should_Have_Error_When_CreatedBy_Exceeds_MaxLength()
        {
            var model = new UserRoleDTO { UserCode = "USR0000001", RoleCode = "ROL0000001", CreatedAt = DateTime.UtcNow, CreatedBy = new string('A', 51), IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedBy)
                .WithErrorMessage("El nombre del usuario creador no puede exceder los 50 caracteres.");
        }

        [Test]
        public void Should_Have_Error_When_UpdatedBy_Exceeds_MaxLength()
        {
            var model = new UserRoleDTO { UserCode = "USR0000001", RoleCode = "ROL0000001", CreatedAt = DateTime.UtcNow, CreatedBy = "admin", UpdatedBy = new string('U', 51), IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UpdatedBy)
                .WithErrorMessage("El nombre del usuario actualizador no puede exceder los 50 caracteres.");
        }

        [Test]
        public void Should_Not_Have_Error_When_Model_Is_Valid()
        {
            var now = DateTime.UtcNow.AddSeconds(-1);
            var model = new UserRoleDTO
            {
                UserCode = "USR0000001",
                RoleCode = "ROL0000001",
                CreatedAt = now,
                UpdatedAt = now.AddMinutes(1),
                CreatedBy = "admin",
                UpdatedBy = "admin2",
                IsActive = true
            };

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}