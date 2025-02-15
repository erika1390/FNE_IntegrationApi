using Integration.Aplication.Validations;
using Integration.Core.Entities.Security;
using Integration.Shared.DTO.Aut;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
namespace Integration.Aplication.Services
{
    public class AuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<bool> RegisterUser(RegisterUserDTO model)
        {
            var validator = new RegisterUserValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Error en validación de usuario: {0}", validationResult.Errors);
                return false;
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
                CreatedBy = "Admin"
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            return result.Succeeded;
        }


        public async Task<bool> AssignRoleToUser(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;

            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new Role { Name = roleName, CreatedBy = "Admin" });

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }
    }
}