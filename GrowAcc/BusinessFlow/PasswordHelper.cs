using System.Text;
using System.Security.Cryptography;

namespace GrowAcc.BusinessFlow
{
    public class PasswordHelper
    {
        public string GenerateSalt(int length = 16)
        {
            var saltBytes = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
        public string HashPasswordWithSalt(string password, string storedSalt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + storedSalt;
                byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
                byte[] hashBytes = sha256.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
        public bool VerifyPassword(string password, string salt, string hashedPassword)
        {
            var hashedInputPassword = HashPasswordWithSalt(password, salt);
            return hashedInputPassword == hashedPassword;
        }
        public string GenerateToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}