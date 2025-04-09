using AutoMapper;

using Integration.Application.Interfaces.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;

using Microsoft.Extensions.Logging;

using System.Linq.Expressions;
namespace Integration.Application.Services.Security
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IAuthenticationService _authenticationService;
        public UserService(IUserRepository repository, IMapper mapper, ILogger<UserService> logger, IAuthenticationService authenticationService)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _authenticationService = authenticationService;
        }
        public async Task<UserDTO> CreateAsync(HeaderDTO header, UserDTO userDTO)
        {
            if (header == null || string.IsNullOrEmpty(header.UserCode))
            {
                throw new ArgumentNullException(nameof(header), "El encabezado o UserCode no pueden ser nulos.");
            }
            if (userDTO == null)
            {
                throw new ArgumentNullException(nameof(userDTO), "El usuario no puede ser nula.");
            }
            _logger.LogInformation("Creando usuario: {Name}", userDTO.UserName);
            try
            {
                var userCreate = await _repository.GetByCodeAsync(header.UserCode);
                if (userCreate == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                var user = _mapper.Map<Integration.Core.Entities.Security.User>(userDTO);
                user.NormalizedUserName = user.UserName.ToUpper();
                user.NormalizedEmail = user.Email.ToUpper();    
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.ConcurrencyStamp = Guid.NewGuid().ToString();
                user.CreatedBy = userCreate.UserName;
                user.UpdatedBy = userCreate.UserName;
                user.PasswordHash = await _authenticationService.GenerarPasswordHashAsync(userDTO.Password);
                var result = await _repository.CreateAsync(user);
                _logger.LogInformation("Usuario creado con éxito: UserCode: {UserCode}, Nombre: {Name}", result.Code, result.UserName);
                return _mapper.Map<UserDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el usuario: {Name}", userDTO.UserName);
                throw;
            }
        }

        public async Task<bool> DeactivateAsync(HeaderDTO header, string code)
        {
            _logger.LogInformation("Desactivar usuario con UserCode: {UserCode}", code);
            try
            {
                var user = await _repository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                bool success = await _repository.DeactivateAsync(code, user.UserName);
                if (success)
                {
                    _logger.LogInformation("Usuario con codigo {UserCode} desactivado correctamente.", code);
                }
                else
                {
                    _logger.LogWarning("No se encontró el usuario con codigo {UserCode} para desactivar.", code);
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar el usuario con codigo {UserCode}.", code);
                throw;
            }
        }

        public async Task<IEnumerable<UserDTO>> GetAllActiveAsync()
        {
            _logger.LogInformation("Obteniendo todos los usuarios.");
            try
            {
                var users = await _repository.GetAllActiveAsync();
                var usersDTO = _mapper.Map<IEnumerable<UserDTO>>(users);
                _logger.LogInformation("{Count} usuarios obtenidos con éxito.", usersDTO.Count());
                return usersDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los usuarios.");
                throw;
            }
        }

        public async Task<List<UserDTO>> GetByFilterAsync(Expression<Func<UserDTO, bool>> predicate)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los usuarios y aplicando el filtro en memoria.");
                var users = await _repository.GetAllAsync(a => true);
                var usersDTOs = _mapper.Map<List<UserDTO>>(users);
                var filteredApplications = usersDTOs.AsQueryable().Where(predicate).ToList();
                return filteredApplications;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener los usuarios.");
                throw;
            }
        }

        public async Task<List<UserDTO>> GetByMultipleFiltersAsync(List<Expression<Func<UserDTO, bool>>> predicates)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los usuarios y aplicando múltiples filtros en memoria.");
                var users = await _repository.GetAllAsync(a => true);
                var usersDTOs = _mapper.Map<List<UserDTO>>(users);
                IQueryable<UserDTO> query = usersDTOs.AsQueryable();
                foreach (var predicado in predicates)
                {
                    query = query.Where(predicado);
                }
                return query.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener los usuarios con múltiples filtros.");
                throw;
            }
        }

        public async Task<UserDTO> GetByCodeAsync(string code)
        {
            _logger.LogInformation("Buscando usuario con UserCode: {UserCode}", code);
            try
            {
                var permission = await _repository.GetByCodeAsync(code);
                if (permission == null)
                {
                    _logger.LogWarning("No se encontró el usuario con UserCode {UserCode}.", code);
                    return null;
                }
                _logger.LogInformation("Usuario encontrada: UserCode:{UserCode}, Nombre: {Name}", permission.Code, permission.UserName);
                return _mapper.Map<UserDTO>(permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuarios con UserCode {UserCode}.", code);
                throw;
            }
        }

        public async Task<UserDTO> UpdateAsync(HeaderDTO header, UserDTO userDTO)
        {
            _logger.LogInformation("Actualizando usuario con UserName: {UserName}", userDTO.UserName);
            try
            {
                var userHeader = await _repository.GetByCodeAsync(header.UserCode);
                if (userHeader == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                var userBody = _mapper.Map<Integration.Core.Entities.Security.User>(userDTO);
                userBody.UpdatedBy = userHeader.UserName;
                var updatedUser = await _repository.UpdateAsync(userBody);
                if (updatedUser == null)
                {
                    _logger.LogWarning("No se pudo actualizar el usuario con UserName {UserName}.", userDTO.UserName);
                    return null;
                }
                _logger.LogInformation("Usuario actualizado con éxito: UserCode {UserCode}, UserName: {UserName}", updatedUser.Code, updatedUser.UserName);
                return _mapper.Map<UserDTO>(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con UserName {UserName}.", userDTO.UserName);
                throw;
            }
        }
    }
}
