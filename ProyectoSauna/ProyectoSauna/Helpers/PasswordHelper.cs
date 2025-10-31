using System.Security.Cryptography;
using System.Text;

namespace ProyectoSauna.Helpers
{
    public static class PasswordHelper
    {
        private const string SALT = "SaunaSalt2024";

        /// <summary>
        /// Genera un hash seguro de la contraseña usando SHA256 y un salt fijo
        /// </summary>
        /// <param name="password">Contraseña en texto plano</param>
        /// <returns>Hash de la contraseña en Base64</returns>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("La contraseña no puede ser nula o vacía", nameof(password));

            using var sha256 = SHA256.Create();
            var saltedPassword = password + SALT;
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            return Convert.ToBase64String(hashedBytes);
        }

        /// <summary>
        /// Verifica si una contraseña coincide con el hash almacenado
        /// </summary>
        /// <param name="password">Contraseña en texto plano</param>
        /// <param name="hash">Hash almacenado</param>
        /// <returns>True si la contraseña es correcta</returns>
        public static bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
                return false;

            var passwordHash = HashPassword(password);
            return passwordHash.Equals(hash, StringComparison.Ordinal);
        }
    }
}