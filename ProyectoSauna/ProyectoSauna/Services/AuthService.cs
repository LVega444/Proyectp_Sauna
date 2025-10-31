using ProyectoSauna.Models.DTOs;
using ProyectoSauna.Repositories.Interfaces;
using ProyectoSauna.Services.Interfaces;
using ProyectoSauna.Helpers;

namespace ProyectoSauna.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AuthService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<LoginResultDTO> ValidarLoginAsync(LoginRequestDTO loginRequest)
        {
            return await ValidarLoginAsync(loginRequest.Usuario, loginRequest.Clave);
        }

        public async Task<LoginResultDTO> ValidarLoginAsync(string usuario, string clave)
        {
            try
            {
                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave))
                {
                    return new LoginResultDTO
                    {
                        IsSuccess = false,
                        ErrorMessage = "Debe ingresar usuario y contraseña."
                    };
                }

                // Hash de la contraseña
                // Ya no necesario hacer hash aquí, se hace en VerifyPassword
                
                // Buscar el usuario por nombre de usuario
                var usuarioEntity = await _usuarioRepository.ObtenerPorNombreUsuarioAsync(usuario);

                if (usuarioEntity == null)
                {
                    return new LoginResultDTO
                    {
                        IsSuccess = false,
                        ErrorMessage = "Usuario o contraseña incorrectos."
                    };
                }

                // Validar contraseña usando el helper
                if (!PasswordHelper.VerifyPassword(clave, usuarioEntity.contraseniaHash))
                {
                    return new LoginResultDTO
                    {
                        IsSuccess = false,
                        ErrorMessage = "Usuario o contraseña incorrectos."
                    };
                }

                // Validar que el usuario esté activo
                if (!usuarioEntity.activo)
                {
                    return new LoginResultDTO
                    {
                        IsSuccess = false,
                        ErrorMessage = "El usuario está inactivo. Contacte al administrador."
                    };
                }

                // Login exitoso
                return new LoginResultDTO
                {
                    IsSuccess = true,
                    NombreUsuario = usuarioEntity.nombreUsuario,
                    Rol = usuarioEntity.idRolNavigation?.nombre ?? "Sin rol",
                    IdUsuario = usuarioEntity.idUsuario
                };
            }
            catch (Exception ex)
            {
                return new LoginResultDTO
                {
                    IsSuccess = false,
                    ErrorMessage = $"Error en el sistema: {ex.Message}"
                };
            }
        }

        public string HashPassword(string password)
        {
            return PasswordHelper.HashPassword(password);
        }

        public async Task<bool> CambiarClaveAsync(int idUsuario, string claveActual, string claveNueva)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByIdAsync(idUsuario);
                if (usuario == null || !usuario.activo)
                    return false;

                // Verificar la clave actual
                if (!PasswordHelper.VerifyPassword(claveActual, usuario.contraseniaHash))
                    return false;

                // Hashear la nueva clave
                usuario.contraseniaHash = PasswordHelper.HashPassword(claveNueva);
                
                await _usuarioRepository.UpdateAsync(usuario);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RestablecerClaveAsync(int idUsuario, string claveNueva)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByIdAsync(idUsuario);
                if (usuario == null)
                    return false;

                // Hashear la nueva clave
                usuario.contraseniaHash = PasswordHelper.HashPassword(claveNueva);
                
                await _usuarioRepository.UpdateAsync(usuario);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}