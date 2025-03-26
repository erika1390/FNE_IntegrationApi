using FluentValidation.TestHelper;

using Integration.Application.Validations.Security;
using Integration.Shared.DTO.Security;

using NUnit.Framework;

using System;

namespace Integration.Application.Test.Validations.Security
{
    [TestFixture]
    public class UserDTOValidatorTest
    {
        private UserDTOValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new UserDTOValidator();
        }

        [Test]
        public void Should_Have_Error_When_Code_Is_Empty()
        {
            var model = new UserDTO { Code = string.Empty, FirstName = "John", LastName = "Doe", CreatedAt = DateTime.UtcNow, CreatedBy = "Admin", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Code).WithErrorMessage("El código del usuario es obligatorio.");
        }

        [Test]
        public void Should_Have_Error_When_Code_Exceeds_MaxLength()
        {
            var model = new UserDTO { Code = new string('A', 11), FirstName = "John", LastName = "Doe", CreatedAt = DateTime.UtcNow, CreatedBy = "Admin", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Code).WithErrorMessage("El código no puede exceder los 10 caracteres.");
        }

        [Test]
        public void Should_Have_Error_When_FirstName_Is_Empty()
        {
            var model = new UserDTO { Code = "USR0000001", FirstName = string.Empty, LastName = "Doe", CreatedAt = DateTime.UtcNow, CreatedBy = "Admin", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.FirstName).WithErrorMessage("El nombre del usuario es obligatorio.");
        }

        [Test]
        public void Should_Have_Error_When_FirstName_Exceeds_MaxLength()
        {
            var model = new UserDTO { Code = "USR0000001", FirstName = new string('A', 101), LastName = "Doe", CreatedAt = DateTime.UtcNow, CreatedBy = "Admin", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.FirstName).WithErrorMessage("El nombre no puede exceder los 100 caracteres.");
        }

        [Test]
        public void Should_Have_Error_When_LastName_Is_Empty()
        {
            var model = new UserDTO { Code = "USR0000001", FirstName = "John", LastName = string.Empty, CreatedAt = DateTime.UtcNow, CreatedBy = "Admin", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.LastName).WithErrorMessage("El apellido del usuario es obligatorio.");
        }

        [Test]
        public void Should_Have_Error_When_LastName_Exceeds_MaxLength()
        {
            var model = new UserDTO { Code = "USR0000001", FirstName = "John", LastName = new string('A', 101), CreatedAt = DateTime.UtcNow, CreatedBy = "Admin", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.LastName).WithErrorMessage("El apellido no puede exceder los 100 caracteres.");
        }

        [Test]
        public void Should_Have_Error_When_UserName_Exceeds_MaxLength()
        {
            var model = new UserDTO { Code = "USR0000001", FirstName = "John", LastName = "Doe", UserName = new string('A', 257), CreatedAt = DateTime.UtcNow, CreatedBy = "Admin", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UserName).WithErrorMessage("El nombre de usuario no puede exceder los 256 caracteres.");
        }

        [Test]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            var model = new UserDTO { Code = "USR0000001", FirstName = "John", LastName = "Doe", Email = "invalid-email", CreatedAt = DateTime.UtcNow, CreatedBy = "Admin", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("El correo electrónico no es válido.");
        }

        [Test]
        public void Should_Have_Error_When_Email_Exceeds_MaxLength()
        {
            var model = new UserDTO { Code = "USR0000001", FirstName = "John", LastName = "Doe", Email = new string('A', 257), CreatedAt = DateTime.UtcNow, CreatedBy = "Admin", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("El correo electrónico no puede exceder los 256 caracteres.");
        }

        [Test]
        public void Should_Have_Error_When_PhoneNumber_Is_Invalid()
        {
            var model = new UserDTO { Code = "USR0000001", FirstName = "John", LastName = "Doe", PhoneNumber = "invalid-phone", CreatedAt = DateTime.UtcNow, CreatedBy = "Admin", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber).WithErrorMessage("El número de teléfono no es válido.");
        }

        [Test]
        public void Should_Have_Error_When_DateOfBirth_Is_In_The_Future()
        {
            var model = new UserDTO { Code = "USR0000001", FirstName = "John", LastName = "Doe", DateOfBirth = DateTime.UtcNow.AddDays(1), CreatedAt = DateTime.UtcNow, CreatedBy = "Admin", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth).WithErrorMessage("La fecha de nacimiento no puede ser en el futuro.");
        }

        [Test]
        public void Should_Have_Error_When_CreatedAt_Is_In_The_Future()
        {
            var model = new UserDTO { Code = "USR0000001", FirstName = "John", LastName = "Doe", CreatedAt = DateTime.UtcNow.AddDays(1), CreatedBy = "Admin", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedAt).WithErrorMessage("La fecha de creación no puede ser en el futuro.");
        }

        [Test]
        public void Should_Have_Error_When_UpdatedAt_Is_Before_CreatedAt()
        {
            var model = new UserDTO { Code = "USR0000001", FirstName = "John", LastName = "Doe", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow.AddDays(-1), CreatedBy = "Admin", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UpdatedAt).WithErrorMessage("La fecha de actualización no puede ser menor a la fecha de creación.");
        }

        [Test]
        public void Should_Have_Error_When_CreatedBy_Is_Empty()
        {
            var model = new UserDTO { Code = "USR0000001", FirstName = "John", LastName = "Doe", CreatedAt = DateTime.UtcNow, CreatedBy = string.Empty, IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedBy).WithErrorMessage("El usuario que creó el registro es obligatorio.");
        }

        [Test]
        public void Should_Have_Error_When_CreatedBy_Exceeds_MaxLength()
        {
            var model = new UserDTO { Code = "USR0000001", FirstName = "John", LastName = "Doe", CreatedAt = DateTime.UtcNow, CreatedBy = new string('A', 51), IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedBy).WithErrorMessage("El nombre del usuario creador no puede exceder los 50 caracteres.");
        }

        [Test]
        public void Should_Have_Error_When_UpdatedBy_Exceeds_MaxLength()
        {
            var model = new UserDTO { Code = "USR0000001", FirstName = "John", LastName = "Doe", CreatedAt = DateTime.UtcNow, CreatedBy = "Admin", UpdatedBy = new string('A', 51), IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UpdatedBy).WithErrorMessage("El nombre del usuario actualizador no puede exceder los 50 caracteres.");
        }
    }
}