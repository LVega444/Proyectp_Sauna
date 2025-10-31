using ProyectoSauna.Models.DTOs;

namespace ProyectoSauna.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResultDTO> ValidarLoginAsync(LoginRequestDTO loginRequest);
        Task<LoginResultDTO> ValidarLoginAsync(string usuario, string clave);
        string HashPassword(string password);
        Task<bool> CambiarClaveAsync(int idUsuario, string claveActual, string claveNueva);
        Task<bool> RestablecerClaveAsync(int idUsuario, string claveNueva);
    }
}