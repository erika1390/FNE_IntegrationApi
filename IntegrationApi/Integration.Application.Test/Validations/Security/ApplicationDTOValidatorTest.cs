using FluentValidation.TestHelper;

using Integration.Application.Validations.Security;
using Integration.Shared.DTO.Security;

namespace Integration.Application.Test.Validations.Security
{
    [TestFixture]
    public class ApplicationDTOValidatorTest
    {
        private ApplicationDTOValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new ApplicationDTOValidator();
        }
                
        [Test]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var model = new ApplicationDTO { Code = "APP0000001", Name = string.Empty, CreatedAt = DateTime.UtcNow, CreatedBy = "User", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("El nombre de la aplicación es obligatorio.");
        }

        [Test]
        public void Should_Have_Error_When_Name_Exceeds_MaxLength()
        {
            var model = new ApplicationDTO { Code = "APP0000001", Name = new string('A', 51), CreatedAt = DateTime.UtcNow, CreatedBy = "User", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("El nombre no puede exceder los 50 caracteres.");
        }

        [Test]
        public void Should_Have_Error_When_CreatedAt_Is_In_The_Future()
        {
            var model = new ApplicationDTO { Code = "APP0000001", Name = "Test", CreatedAt = DateTime.UtcNow.AddDays(1), CreatedBy = "User", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedAt).WithErrorMessage("La fecha de creación no puede ser en el futuro.");
        }

        [Test]
        public void Should_Have_Error_When_UpdatedAt_Is_Before_CreatedAt()
        {
            var model = new ApplicationDTO { Code = "APP0000001", Name = "Test", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow.AddDays(-1), CreatedBy = "User", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UpdatedAt).WithErrorMessage("La fecha de actualización no puede ser menor a la fecha de creación.");
        }

        [Test]
        public void Should_Have_Error_When_CreatedBy_Is_Empty()
        {
            var model = new ApplicationDTO { Code = "APP0000001", Name = "Test", CreatedAt = DateTime.UtcNow, CreatedBy = string.Empty, IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedBy).WithErrorMessage("El usuario que creó la aplicación es obligatorio.");
        }

        [Test]
        public void Should_Have_Error_When_CreatedBy_Exceeds_MaxLength()
        {
            var model = new ApplicationDTO { Code = "APP0000001", Name = "Test", CreatedAt = DateTime.UtcNow, CreatedBy = new string('A', 51), IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedBy).WithErrorMessage("El nombre del usuario no puede exceder los 50 caracteres.");
        }

        [Test]
        public void Should_Have_Error_When_UpdatedBy_Is_Empty()
        {
            var model = new ApplicationDTO { Code = "APP0000001", Name = "Test", CreatedAt = DateTime.UtcNow, CreatedBy = "User", UpdatedBy = string.Empty, IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UpdatedBy).WithErrorMessage("El usuario que actualizó la aplicación es obligatorio.");
        }

        [Test]
        public void Should_Have_Error_When_UpdatedBy_Exceeds_MaxLength()
        {
            var model = new ApplicationDTO { Code = "APP0000001", Name = "Test", CreatedAt = DateTime.UtcNow, CreatedBy = "User", UpdatedBy = new string('A', 51), IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UpdatedBy).WithErrorMessage("El nombre del usuario no puede exceder los 50 caracteres.");
        }
    }
}