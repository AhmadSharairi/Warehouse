using System.Security.Cryptography;

namespace API.Helper
{
    public class PasswordHasher
    {
        private static readonly int SaltSize = 16;
        private static readonly int HashSize = 20;
        private static readonly int Iterations = 1000;

     


        public static string HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);  

            var key = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256); 
            var hash = key.GetBytes(HashSize);

            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            return Convert.ToBase64String(hashBytes);
        }




        //verify the password from the database after hashing
        public static bool VerifyPassword(string inputPassword, string base64PasswordHashed)
        {
            var hashBytes = Convert.FromBase64String(base64PasswordHashed);
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            var key = new Rfc2898DeriveBytes(inputPassword, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] hash = key.GetBytes(HashSize);

            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                    return false;
            }

            return true;
        }
    }
}
