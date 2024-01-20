using System.Security.Cryptography;
using System.Text;

namespace Authorization.Security
{
    public class PasswordHash
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Преобразование строки в массив байтов
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Хеширование пароля
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);

                // Преобразование массива байтов обратно в строку
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
