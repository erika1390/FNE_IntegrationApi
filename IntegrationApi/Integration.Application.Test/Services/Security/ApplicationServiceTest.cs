﻿using AutoMapper;
using Integration.Application.Interfaces.Security;
using Integration.Application.Services.Security;
using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
namespace Integration.Application.Test.Services.Security
{
    [TestFixture]
    public class ApplicationServiceTest
    {
        private Mock<IApplicationRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<ApplicationService>> _loggerMock;
        private IApplicationService _applicationService;
        private Mock<IUserRepository> _userRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IApplicationRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ApplicationService>>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _applicationService = new ApplicationService(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object, _userRepositoryMock.Object);
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedApplicationDTO()
        {
            // Arrange
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            var applicationDTO = new ApplicationDTO { Name = "Saga 2.0", Code = "APP0000001", CreatedBy = "System", IsActive = true };
            var application = new Integration.Core.Entities.Security.Application { Id = 1, Name = "Saga 2.0", Code = "APP0000001" };

            // ✅ Simular un usuario válido en el repositorio
            var user = new User {
                Code = "USR0000001",
                FirstName = "Erika",
                LastName = "Pulido Moreno",
                DateOfBirth = DateTime.Now.AddYears(-34),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = "System",
                IsActive = true,
                UserName = "epulido",
                NormalizedUserName = "EPULIDO",
                Email = "epulido@minsalud.gov.co",
                NormalizedEmail = "EPULIDO@MINSALUD.GOV.CO",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEJ9zQ6",
                SecurityStamp = "QJZQ4Q",
                ConcurrencyStamp = "d1b1b2b3-4b5b-6b7b-8b9b-0b1b2b3b4b5",
                PhoneNumber = "3001234567",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0
            };
            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);

            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Security.Application>(applicationDTO)).Returns(application);
            _repositoryMock.Setup(r => r.CreateAsync(application)).ReturnsAsync(application);
            _mapperMock.Setup(m => m.Map<ApplicationDTO>(application)).Returns(applicationDTO);

            // Act
            var result = await _applicationService.CreateAsync(header, applicationDTO);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(applicationDTO, result);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnTrue_WhenApplicationIsDeactivated()
        {
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            string applicationCode = "APP0000001";
            _repositoryMock.Setup(r => r.DeactivateAsync(applicationCode)).ReturnsAsync(true);

            var result = await _applicationService.DeactivateAsync(header, applicationCode);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnFalse_WhenApplicationIsNotFound()
        {
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            string applicationCode = "APP0000001";
            _repositoryMock.Setup(r => r.DeactivateAsync(applicationCode)).ReturnsAsync(false);

            var result = await _applicationService.DeactivateAsync(header, applicationCode);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnListOfApplicationDTOs()
        {
            var applications = new List<Integration.Core.Entities.Security.Application> { new Integration.Core.Entities.Security.Application { Id = 1, Name = "Saga 2.0", Code = "APP0000001" } };
            var applicationDTOs = new List<ApplicationDTO> { new ApplicationDTO { Name = "Saga 2.0", Code = "APP0000001", CreatedBy = "System", IsActive = true } };

            _repositoryMock.Setup(r => r.GetAllActiveAsync()).ReturnsAsync(applications);
            _mapperMock.Setup(m => m.Map<IEnumerable<ApplicationDTO>>(applications)).Returns(applicationDTOs);

            var result = await _applicationService.GetAllActiveAsync();

            Assert.AreEqual(applicationDTOs, result);
        }

        [Test]
        public async Task GetAllAsync_WithSinglePredicate_ShouldReturnFilteredApplicationDTOs()
        {
            var applications = new List<Integration.Core.Entities.Security.Application> { new Integration.Core.Entities.Security.Application { Id = 1, Name = "Saga 2.0", Code = "APP0000001" } };
            var applicationDTOs = new List<ApplicationDTO> { new ApplicationDTO { Name = "Saga 2.0", Code = "APP0000001", CreatedBy = "System", IsActive = true } };
            Expression<Func<ApplicationDTO, bool>> filter = dto => dto.Name == "Saga 2.0";

            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Integration.Core.Entities.Security.Application, bool>>>())).ReturnsAsync(applications);
            _mapperMock.Setup(m => m.Map<List<ApplicationDTO>>(applications)).Returns(applicationDTOs);

            var result = await _applicationService.GetAllAsync(filter);

            Assert.AreEqual(applicationDTOs, result);
        }

        [Test]
        public async Task GetAllAsync_WithMultiplePredicates_ShouldReturnFilteredApplicationDTOs()
        {
            var applications = new List<Integration.Core.Entities.Security.Application> { new Integration.Core.Entities.Security.Application { Id = 1, Name = "Saga 2.0", Code = "APP0000001" } };
            var applicationDTOs = new List<ApplicationDTO> { new ApplicationDTO { Name = "Saga 2.0", Code = "APP0000001", CreatedBy = "System", IsActive = true } };
            var predicates = new List<Expression<Func<ApplicationDTO, bool>>> { dto => dto.Name == "Saga 2.0", dto => dto.Code == "APP0000001" };

            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Integration.Core.Entities.Security.Application, bool>>>())).ReturnsAsync(applications);
            _mapperMock.Setup(m => m.Map<List<ApplicationDTO>>(applications)).Returns(applicationDTOs);

            var result = await _applicationService.GetAllAsync(predicates);

            Assert.AreEqual(applicationDTOs, result);
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnApplicationDTO_WhenApplicationExists()
        {
            var application = new Integration.Core.Entities.Security.Application { Id = 1, Name = "Saga 2.0", Code = "APP0000001" };
            var applicationDTO = new ApplicationDTO { Name = "Saga 2.0", Code = "APP0000001", CreatedBy = "System", IsActive = true };

            _repositoryMock.Setup(r => r.GetByCodeAsync("APP0000001")).ReturnsAsync(application);
            _mapperMock.Setup(m => m.Map<ApplicationDTO>(application)).Returns(applicationDTO);

            var result = await _applicationService.GetByCodeAsync("APP0000001");

            Assert.AreEqual(applicationDTO, result);
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnNull_WhenApplicationDoesNotExist()
        {
            _repositoryMock.Setup(r => r.GetByCodeAsync("APP0000001")).ReturnsAsync((Integration.Core.Entities.Security.Application)null);

            var result = await _applicationService.GetByCodeAsync("APP0000001");

            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedApplicationDTO()
        {
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            var applicationDTO = new ApplicationDTO { Name = "Saga 2.0", Code = "APP0000001", CreatedBy = "User", IsActive = true };
            var application = new Integration.Core.Entities.Security.Application { Id = 1, Name = "Saga 2.0", Code = "APP0000001" };

            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Security.Application>(applicationDTO)).Returns(application);
            _repositoryMock.Setup(r => r.UpdateAsync(application)).ReturnsAsync(application);
            _mapperMock.Setup(m => m.Map<ApplicationDTO>(application)).Returns(applicationDTO);

            var result = await _applicationService.UpdateAsync(header, applicationDTO);

            Assert.AreEqual(applicationDTO, result);
        }
    }
}