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
        public UserService(IUserRepository repository, IMapper mapper, ILogger<UserService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<UserDTO> CreateAsync(HeaderDTO header, UserDTO userDTO)
        {
            _logger.LogInformation("Creando usuario: {Name}", userDTO.UserName);
            try
            {
                var user = _mapper.Map<Integration.Core.Entities.Security.User>(userDTO);
                user.NormalizedUserName = user.UserName.ToUpper();
                user.NormalizedEmail = user.Email.ToUpper();    
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.ConcurrencyStamp = Guid.NewGuid().ToString();
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
            _logger.LogInformation("Eliminando usuario con UserCode: {UserCode}", code);
            try
            {
                bool success = await _repository.DeactivateAsync(code);
                if (success)
                {
                    _logger.LogInformation("Usuario con UserCode {UserCode} eliminada correctamente.", code);
                }
                else
                {
                    _logger.LogWarning("No se encontró el usuario con UserCode {UserCode} para eliminar.", code);
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario con UserCode {UserCode}.", code);
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

        public async Task<List<UserDTO>> GetAllAsync(Expression<Func<UserDTO, bool>> filterDto)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los usuarios y aplicando el filtro en memoria.");
                var users = await _repository.GetAllAsync(a => true);
                var usersDTOs = _mapper.Map<List<UserDTO>>(users);
                var filteredApplications = usersDTOs.AsQueryable().Where(filterDto).ToList();
                return filteredApplications;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener los usuarios.");
                throw;
            }
        }

        public async Task<List<UserDTO>> GetAllAsync(List<Expression<Func<UserDTO, bool>>> predicados)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los usuarios y aplicando múltiples filtros en memoria.");
                var users = await _repository.GetAllAsync(a => true);
                var usersDTOs = _mapper.Map<List<UserDTO>>(users);
                IQueryable<UserDTO> query = usersDTOs.AsQueryable();
                foreach (var predicado in predicados)
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
                var user = _mapper.Map<Integration.Core.Entities.Security.User>(userDTO);
                var updatedUser = await _repository.UpdateAsync(user);
                if (updatedUser == null)
                {
                    _logger.LogWarning("No se pudo actualizar el usuario con UserName {UserName}.", userDTO.UserName);
                    return null;
                }
                _logger.LogInformation("Usuario actualizado con éxito: {UserId}, UserName: {UserName}", updatedUser.Id, updatedUser.UserName);
                return _mapper.Map<UserDTO>(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID {UserName}.", userDTO.UserName);
                throw;
            }
        }
    }
}
