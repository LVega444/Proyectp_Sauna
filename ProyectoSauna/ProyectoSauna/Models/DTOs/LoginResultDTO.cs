namespace ProyectoSauna.Models.DTOs
{
    public class LoginResultDTO
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Rol { get; set; }
        public int? IdUsuario { get; set; }
    }
}