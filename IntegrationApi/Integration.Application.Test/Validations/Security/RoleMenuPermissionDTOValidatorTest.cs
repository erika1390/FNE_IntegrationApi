using FluentValidation.TestHelper;

using Integration.Application.Validations.Security;
using Integration.Shared.DTO.Security;

using NUnit.Framework;

using System;

namespace Integration.Application.Test.Validations.Security
{
    [TestFixture]
    public class RoleMenuPermissionDTOValidatorTest
    {
        private RoleMenuPermissionDTOValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new RoleMenuPermissionDTOValidator();
        }

        [Test]
        public void Should_Have_Error_When_RoleCode_Is_Empty()
        {
            var model = new RoleMenuPermissionDTO { RoleCode = string.Empty, MenuCode = "MOD0000001", PermissionCode = "PER0000001", CreatedAt = DateTime.UtcNow, CreatedBy = "User", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.RoleCode).WithErrorMessage("El código del rol es obligatorio.");
        }

        [Test]
        public void Should_Have_Error_When_RoleCode_Exceeds_MaxLength()
        {
            var model = new RoleMenuPermissionDTO { RoleCode = new string('A', 11), MenuCode = "MOD0000001", PermissionCode = "PER0000001", CreatedAt = DateTime.UtcNow, CreatedBy = "User", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.RoleCode).WithErrorMessage("El código del rol no puede exceder los 10 caracteres.");
        }

        [Test]
        public void Should_Have_Error_When_ModuleCode_Is_Empty()
        {
            var model = new RoleMenuPermissionDTO { RoleCode = "ROL0000001", MenuCode = string.Empty, PermissionCode = "PER0000001", CreatedAt = DateTime.UtcNow, CreatedBy = "User", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MenuCode).WithErrorMessage("El código del módulo es obligatorio.");
        }

        [Test]
        public void Should_Have_Error_When_ModuleCode_Exceeds_MaxLength()
        {
            var model = new RoleMenuPermissionDTO { RoleCode = "ROL0000001", MenuCode = new string('A', 11), PermissionCode = "PER0000001", CreatedAt = DateTime.UtcNow, CreatedBy = "User", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MenuCode).WithErrorMessage("El código del módulo no puede exceder los 10 caracteres.");
        }

        [Test]
        public void Should_Have_Error_When_PermissionCode_Is_Empty()
        {
            var model = new RoleMenuPermissionDTO { RoleCode = "ROL0000001", MenuCode = "MOD0000001", PermissionCode = string.Empty, CreatedAt = DateTime.UtcNow, CreatedBy = "User", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PermissionCode).WithErrorMessage("El código del permiso es obligatorio.");
        }

        [Test]
        public void Should_Have_Error_When_PermissionCode_Exceeds_MaxLength()
        {
            var model = new RoleMenuPermissionDTO { RoleCode = "ROL0000001", MenuCode = "MOD0000001", PermissionCode = new string('A', 11), CreatedAt = DateTime.UtcNow, CreatedBy = "User", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PermissionCode).WithErrorMessage("El código del permiso no puede exceder los 10 caracteres.");
        }

        [Test]
        public void Should_Have_Error_When_CreatedAt_Is_In_The_Future()
        {
            var model = new RoleMenuPermissionDTO { RoleCode = "ROL0000001", MenuCode = "MOD0000001", PermissionCode = "PER0000001", CreatedAt = DateTime.UtcNow.AddDays(1), CreatedBy = "User", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedAt).WithErrorMessage("La fecha de creación no puede ser en el futuro.");
        }

        [Test]
        public void Should_Have_Error_When_UpdatedAt_Is_Before_CreatedAt()
        {
            var model = new RoleMenuPermissionDTO { RoleCode = "ROL0000001", MenuCode = "MOD0000001", PermissionCode = "PER0000001", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow.AddDays(-1), CreatedBy = "User", IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UpdatedAt).WithErrorMessage("La fecha de actualización no puede ser menor a la fecha de creación.");
        }

        [Test]
        public void Should_Have_Error_When_CreatedBy_Is_Empty()
        {
            var model = new RoleMenuPermissionDTO { RoleCode = "ROL0000001", MenuCode = "MOD0000001", PermissionCode = "PER0000001", CreatedAt = DateTime.UtcNow, CreatedBy = string.Empty, IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedBy).WithErrorMessage("El usuario que creó el registro es obligatorio.");
        }

        [Test]
        public void Should_Have_Error_When_CreatedBy_Exceeds_MaxLength()
        {
            var model = new RoleMenuPermissionDTO { RoleCode = "ROL0000001", MenuCode = "MOD0000001", PermissionCode = "PER0000001", CreatedAt = DateTime.UtcNow, CreatedBy = new string('A', 51), IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedBy).WithErrorMessage("El nombre del usuario creador no puede exceder los 50 caracteres.");
        }

        [Test]
        public void Should_Have_Error_When_UpdatedBy_Exceeds_MaxLength()
        {
            var model = new RoleMenuPermissionDTO { RoleCode = "ROL0000001", MenuCode = "MOD0000001", PermissionCode = "PER0000001", CreatedAt = DateTime.UtcNow, CreatedBy = "User", UpdatedBy = new string('A', 51), IsActive = true };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UpdatedBy).WithErrorMessage("El nombre del usuario actualizador no puede exceder los 50 caracteres.");
        }
    }
}