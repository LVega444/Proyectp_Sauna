namespace ProyectoSauna.Models.DTOs
{
    public class LoginRequestDTO
    {
        public string Usuario { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
    }
}