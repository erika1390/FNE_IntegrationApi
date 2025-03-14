﻿using AutoMapper;
using Integration.Application.Interfaces.Security;
using Integration.Application.Services.Security;
using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Security;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
namespace Integration.Application.Test.Services.Security
{
    [TestFixture]
    public class PermissionServiceTest
    {
        private Mock<IPermissionRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<PermissionService>> _loggerMock;
        private IPermissionService _permissionService;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IPermissionRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<PermissionService>>();
            _permissionService = new PermissionService(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedPermissionDTO()
        {
            var permissionDTO = new PermissionDTO { Name = "Consultar Aplicacion", Code = "PER0000001", CreatedBy = "User", IsActive = true };
            var permission = new Integration.Core.Entities.Security.Permission { Id = 1, Name = "Consultar Aplicacion", Code = "PER0000001" };

            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Security.Permission>(permissionDTO)).Returns(permission);
            _repositoryMock.Setup(r => r.CreateAsync(permission)).ReturnsAsync(permission);
            _mapperMock.Setup(m => m.Map<PermissionDTO>(permission)).Returns(permissionDTO);

            var result = await _permissionService.CreateAsync(permissionDTO);

            Assert.AreEqual(permissionDTO, result);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnTrue_WhenPermissionIsDeleted()
        {
            string permissionCode = "PER0000001";
            _repositoryMock.Setup(r => r.DeactivateAsync(permissionCode)).ReturnsAsync(true);

            var result = await _permissionService.DeactivateAsync(permissionCode);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnFalse_WhenPermissionIsNotFound()
        {
            string permissionCode = "";
            _repositoryMock.Setup(r => r.DeactivateAsync(permissionCode)).ReturnsAsync(false);

            var result = await _permissionService.DeactivateAsync(permissionCode);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnListOfPermissionDTOs()
        {
            var permissions = new List<Integration.Core.Entities.Security.Permission> { new Integration.Core.Entities.Security.Permission { Id = 1, Name = "Consultar Aplicacion", Code = "PER0000001" } };
            var permissionDTOs = new List<PermissionDTO> { new PermissionDTO { Name = "Consultar Aplicacion", Code = "PER0000001", CreatedBy = "User", IsActive = true } };

            _repositoryMock.Setup(r => r.GetAllActiveAsync()).ReturnsAsync(permissions);
            _mapperMock.Setup(m => m.Map<IEnumerable<PermissionDTO>>(permissions)).Returns(permissionDTOs);

            var result = await _permissionService.GetAllActiveAsync();

            Assert.AreEqual(permissionDTOs, result);
        }
        [Test]
        public async Task GetByCodeAsync_ShouldReturnModuleDTO_WhenPermissionExists()
        {
            var permission = new Integration.Core.Entities.Security.Permission { Id = 1, Name = "Consultar", Code = "PER0000001" };
            var permissionDTO = new PermissionDTO { Name = "Consultar", Code = "PER0000001", CreatedBy = "System", IsActive = true};

            _repositoryMock.Setup(r => r.GetByCodeAsync("PER0000001")).ReturnsAsync(permission);
            _mapperMock.Setup(m => m.Map<PermissionDTO>(permission)).Returns(permissionDTO);

            var result = await _permissionService.GetByCodeAsync("PER0000001");

            Assert.AreEqual(permissionDTO, result);
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnNull_WhenPermissionDoesNotExist()
        {
            _repositoryMock.Setup(r => r.GetByCodeAsync("PER0000001")).ReturnsAsync((Integration.Core.Entities.Security.Permission)null);

            var result = await _permissionService.GetByCodeAsync("PER0000001");

            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedPermissionDTO()
        {
            // Arrange
            var permissionDTO = new PermissionDTO { Code = "PER0000001", Name = "Consultar", IsActive = true, CreatedBy= "System"};
            var permissionEntity = new Permission { Id = 1, Code = "PER0000001", Name = "Consultar", IsActive = true, CreatedBy = "System"};
            // Configuración del Mock del repositorio para devolver un permiso válido
            _repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Permission>()))
                           .ReturnsAsync(permissionEntity);
            // Configuración del Mock del Mapper para convertir entre DTO y entidad
            _mapperMock.Setup(m => m.Map<Permission>(It.IsAny<PermissionDTO>())).Returns(permissionEntity);
            _mapperMock.Setup(m => m.Map<PermissionDTO>(It.IsAny<Permission>())).Returns(permissionDTO);
            // Act
            var result = await _permissionService.UpdateAsync(permissionDTO);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(permissionDTO.Code, result.Code);
            Assert.AreEqual(permissionDTO.Name, result.Name);
        }


        [Test]
        public async Task GetAllAsync_WithSinglePredicate_ShouldReturnFilteredPermissionDTOs()
        {
            var permissions = new List<Integration.Core.Entities.Security.Permission> { new Integration.Core.Entities.Security.Permission { Id = 1, Name = "Consultar Aplicacion", Code = "PER0000001" } };
            var permissionDTOs = new List<PermissionDTO> { new PermissionDTO { Name = "Consultar Aplicacion", Code = "PER0000001", CreatedBy = "User", IsActive = true } };
            Expression<Func<PermissionDTO, bool>> filter = dto => dto.Name == "Consultar Aplicacion";

            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Integration.Core.Entities.Security.Permission, bool>>>())).ReturnsAsync(permissions);
            _mapperMock.Setup(m => m.Map<List<PermissionDTO>>(permissions)).Returns(permissionDTOs);

            var result = await _permissionService.GetAllAsync(filter);

            Assert.AreEqual(permissionDTOs, result);
        }

        [Test]
        public async Task GetAllAsync_WithMultiplePredicates_ShouldReturnFilteredPermissionDTOs()
        {
            var permissions = new List<Integration.Core.Entities.Security.Permission> { new Integration.Core.Entities.Security.Permission { Id = 1, Name = "Consultar Aplicacion", Code = "PER0000001" } };
            var permissionDTOs = new List<PermissionDTO> { new PermissionDTO { Name = "Consultar Aplicacion", Code = "PER0000001", CreatedBy = "User", IsActive = true } };
            var predicates = new List<Expression<Func<PermissionDTO, bool>>> { dto => dto.Name == "Consultar Aplicacion", dto => dto.Code == "PER0000001" };

            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Integration.Core.Entities.Security.Permission, bool>>>())).ReturnsAsync(permissions);
            _mapperMock.Setup(m => m.Map<List<PermissionDTO>>(permissions)).Returns(permissionDTOs);

            var result = await _permissionService.GetAllAsync(predicates);

            Assert.AreEqual(permissionDTOs, result);
        }
    }
}