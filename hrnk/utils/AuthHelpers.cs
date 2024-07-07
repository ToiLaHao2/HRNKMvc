using System.Security.Cryptography;
using System.Text;
using BCrypt;

namespace hrnk.utils
{
    public class AuthHelpers
    {
        public async Task<byte[]> MakeHashPassword(string value)
        {
            string hashPassword = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(value, 10));
            byte[] hashedBytes = Encoding.UTF8.GetBytes(hashPassword);
            return hashedBytes;
        }

        public async Task<bool> ComparePasswords(string password, byte[] hashedPassword)
        {
            string resultHashedPassword = Encoding.UTF8.GetString(hashedPassword);
            bool isPasswordValid = await Task.Run(() => BCrypt.Net.BCrypt.Verify(password, resultHashedPassword));

            return isPasswordValid;
        }
    }
}
