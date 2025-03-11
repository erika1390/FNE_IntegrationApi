using AutoMapper;
using Integration.Application.Interfaces.Security;
using Integration.Application.Services.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Security;

using Microsoft.AspNetCore.Mvc.ApplicationModels;
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

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IApplicationRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ApplicationService>>();
            _applicationService = new ApplicationService(_repositoryMock.Object,_mapperMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedApplicationDTO()
        {
            var applicationDTO = new ApplicationDTO { Name = "Saga 2.0", Code = "APP0000001", CreatedBy = "System", IsActive = true };
            var application = new Integration.Core.Entities.Security.Application { Id = 1, Name = "Saga 2.0", Code = "APP0000001" };

            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Security.Application>(applicationDTO)).Returns(application);
            _repositoryMock.Setup(r => r.CreateAsync(application)).ReturnsAsync(application);
            _mapperMock.Setup(m => m.Map<ApplicationDTO>(application)).Returns(applicationDTO);

            var result = await _applicationService.CreateAsync(applicationDTO);

            Assert.AreEqual(applicationDTO, result);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnTrue_WhenApplicationIsDeleted()
        {
            string applicationCode = "APP0000001";
            _repositoryMock.Setup(r => r.DeactivateAsync(applicationCode)).ReturnsAsync(true);

            var result = await _applicationService.DeleteAsync(applicationCode);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnFalse_WhenApplicationIsNotFound()
        {
            string applicationCode  = "APP0000001";
            _repositoryMock.Setup(r => r.DeactivateAsync(applicationCode)).ReturnsAsync(false);

            var result = await _applicationService.DeleteAsync(applicationCode);

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
        public async Task UpdateAsync_ShouldReturnUpdatedApplicationDTO()
        {
            var applicationDTO = new ApplicationDTO { Name = "Saga 2.0", Code = "APP0000001", CreatedBy = "User", IsActive = true };
            var application = new Integration.Core.Entities.Security.Application { Id = 1, Name = "Saga 2.0", Code = "APP0000001" };

            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Security.Application>(applicationDTO)).Returns(application);
            _repositoryMock.Setup(r => r.UpdateAsync(application)).ReturnsAsync(application);
            _mapperMock.Setup(m => m.Map<ApplicationDTO>(application)).Returns(applicationDTO);

            var result = await _applicationService.UpdateAsync(applicationDTO);

            Assert.AreEqual(applicationDTO, result);
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
    }
}