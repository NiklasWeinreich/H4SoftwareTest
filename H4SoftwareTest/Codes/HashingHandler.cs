using BCrypt.Net;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography;
using System.Text;

namespace H4SoftwareTest.Codes
{
    public static class HashingHandler
    {
        public static string MD5Hashing(string textToHash)
        {
            MD5 md5 = MD5.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes(textToHash);
            byte[] hashedValue = md5.ComputeHash(inputBytes);

            return Convert.ToBase64String(hashedValue);
        }

        public static string SHAHashing(string textToHash)
        {
            SHA256 sha256 = SHA256.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes(textToHash);
            byte[] hashedValue = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(hashedValue);
        }

        public static string HMACHashing(string textToHash)
        {
            byte[] myKey = Encoding.ASCII.GetBytes("BCRYPTERBEDRE");
            byte[] inputBytes = Encoding.ASCII.GetBytes(textToHash);

            HMACSHA256 hmac = new HMACSHA256();
            hmac.Key = myKey;

            byte[] hashedValue = hmac.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashedValue);
        }

        public static string PBKDF2Hashing(string textToHash)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(textToHash);
            byte[] salt = Encoding.ASCII.GetBytes("BCRYPTERBEDRE");
            var hashAlgorythm = new HashAlgorithmName("SHA256");
            int itirationer = 10;
            int outputLength = 32;

            byte[] hashedValue = Rfc2898DeriveBytes.Pbkdf2(inputBytes, salt, itirationer, hashAlgorythm, outputLength);
            return Convert.ToBase64String(hashedValue);
        }

        // Download nuGget pakke "BCrypt.Net-Next"
        public static string BCryptHashing(string textToHash)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            bool enhanceEntropy = true;
            HashType hashType = HashType.SHA256;
            return BCrypt.Net.BCrypt.HashPassword(textToHash, salt, enhanceEntropy, hashType);
        }

        public static bool BCryptVerifyHashing(string textToHash, string hashedValueFromDb)
        {
            bool enhanceEntropy = true;
            HashType hashType = HashType.SHA256;
            return BCrypt.Net.BCrypt.Verify(textToHash, hashedValueFromDb, enhanceEntropy, hashType);

        }
    }
}
