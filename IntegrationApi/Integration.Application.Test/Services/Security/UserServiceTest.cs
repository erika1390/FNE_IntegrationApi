﻿using AutoMapper;
using Integration.Application.Interfaces.Security;
using Integration.Application.Services.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Security;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
namespace Integration.Application.Test.Services.Security
{
    [TestFixture]
    public class UserServiceTest
    {
        private Mock<IUserRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<UserService>> _loggerMock;
        private IUserService _userService;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _userService = new UserService(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedUserDTO()
        {
            var userDTO = new UserDTO {
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
            var user = new Integration.Core.Entities.Security.User {
                Id = 1,
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

            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Security.User>(userDTO)).Returns(user);
            _repositoryMock.Setup(r => r.CreateAsync(user)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserDTO>(user)).Returns(userDTO);

            var result = await _userService.CreateAsync(userDTO);

            Assert.AreEqual(userDTO, result);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnTrue_WhenUserIsDeleted()
        {
            int userId = 1;
            _repositoryMock.Setup(r => r.DeleteAsync(userId)).ReturnsAsync(true);

            var result = await _userService.DeactivateAsync(userId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnFalse_WhenUserIsNotFound()
        {
            int userId = 1;
            _repositoryMock.Setup(r => r.DeleteAsync(userId)).ReturnsAsync(false);

            var result = await _userService.DeactivateAsync(userId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnListOfUserDTOs()
        {
            var users = new List<Integration.Core.Entities.Security.User> { 
                new Integration.Core.Entities.Security.User { 
                    Id = 1,
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
                } 
            };
            var userDTOs = new List<UserDTO> { 
                new UserDTO {
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
                } 
            };

            _repositoryMock.Setup(r => r.GetAllActiveAsync()).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<IEnumerable<UserDTO>>(users)).Returns(userDTOs);

            var result = await _userService.GetAllActiveAsync();

            Assert.AreEqual(userDTOs, result);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnUserDTO_WhenUserExists()
        {
            int userId = 1;
            var user = new Integration.Core.Entities.Security.User {
                Id = 1,
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
            var userDTO = new UserDTO {
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
            _repositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserDTO>(user)).Returns(userDTO);
            var result = await _userService.GetByIdAsync(userId);
            Assert.AreEqual(userDTO, result);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            int userId = 1;
            _repositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((Integration.Core.Entities.Security.User)null);
            var result = await _userService.GetByIdAsync(userId);
            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedUserDTO()
        {
            var userDTO = new UserDTO {
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
            var user = new Integration.Core.Entities.Security.User {
                Id = 1,
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
            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Security.User>(userDTO)).Returns(user);
            _repositoryMock.Setup(r => r.UpdateAsync(user)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserDTO>(user)).Returns(userDTO);
            var result = await _userService.UpdateAsync(userDTO);
            Assert.AreEqual(userDTO, result);
        }

        [Test]
        public async Task GetAllAsync_WithSinglePredicate_ShouldReturnFilteredUserDTOs()
        {
            var users = new List<Integration.Core.Entities.Security.User> { 
                new Integration.Core.Entities.Security.User {
                    Id = 1,
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
                } 
            };
            var userDTOs = new List<UserDTO> { 
                new UserDTO {
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
                } 
            };
            Expression<Func<UserDTO, bool>> filter = dto => dto.UserName == "epulido";
            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Integration.Core.Entities.Security.User, bool>>>())).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<List<UserDTO>>(users)).Returns(userDTOs);
            var result = await _userService.GetAllAsync(filter);
            Assert.AreEqual(userDTOs, result);
        }

        [Test]
        public async Task GetAllAsync_WithMultiplePredicates_ShouldReturnFilteredUserDTOs()
        {
            var users = new List<Integration.Core.Entities.Security.User> { 
                new Integration.Core.Entities.Security.User {
                    Id = 1,
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
                } 
            };
            var userDTOs = new List<UserDTO> { 
                new UserDTO {
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
                } 
            };
            var predicates = new List<Expression<Func<UserDTO, bool>>> { dto => dto.UserName == "epulido", dto => dto.IsActive };
            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Integration.Core.Entities.Security.User, bool>>>())).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<List<UserDTO>>(users)).Returns(userDTOs);
            var result = await _userService.GetAllAsync(predicates);
            Assert.AreEqual(userDTOs, result);
        }
    }
}