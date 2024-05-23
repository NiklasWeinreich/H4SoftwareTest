using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace H4SoftwareTest.Codes
{
    public class EncryptionHandler
    {
        private readonly IDataProtector _dataProtector;
        private readonly HttpClient _httpClient;
        private readonly string _privateKeyPath = "privateKey.xml";
        private readonly string _publicKeyPath = "publicKey.xml";
        private string _privateKey;
        private string _publicKey;

        public EncryptionHandler(IDataProtectionProvider dataProtector, HttpClient httpClient)
        {
            _httpClient = httpClient;
            if (File.Exists(_privateKeyPath) && File.Exists(_publicKeyPath))
            {
                // Load keys from files
                _privateKey = File.ReadAllText(_privateKeyPath);
                _publicKey = File.ReadAllText(_publicKeyPath);
            }
            else
            {
                // Generate new keys and save them to files
                using (var rsa = new RSACryptoServiceProvider(2048))
                {
                    _privateKey = rsa.ToXmlString(true);
                    _publicKey = rsa.ToXmlString(false);

                    File.WriteAllText(_privateKeyPath, _privateKey);
                    File.WriteAllText(_publicKeyPath, _publicKey);
                }
            }

            _dataProtector = dataProtector.CreateProtector(_privateKey);
        }

        #region Symmetric Encryption
        public string EncryptSymmetric(string txtToEncrypt) => _dataProtector.Protect(txtToEncrypt);

        public string DecryptSymmetric(string txtToDecrypt) => _dataProtector.Unprotect(txtToDecrypt);
        #endregion

        #region Asymmetric Encryption
        //public async Task<string> EncryptAsymmetricParent(string txtToEncrypt)
        //{
        //    string[] data = new string[2] { txtToEncrypt, _publicKey };
        //    string serializedValue = JsonConvert.SerializeObject(data);
        //    StringContent content = new StringContent(serializedValue, Encoding.UTF8, "application/json");

        //    var response = await _httpClient.PostAsync("https://localhost:7040/api/Encrypt", content);
        //    string encryptedValue = await response.Content.ReadAsStringAsync();

        //    return encryptedValue;
        //}

        public string DecryptAsymmetric(string txtToDecrypt)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(_privateKey);

            byte[] txtToDecryptAsByteArray = Convert.FromBase64String(txtToDecrypt);
            byte[] decryptedValue = rsa.Decrypt(txtToDecryptAsByteArray, true);
            string decryptedValueAsString = Encoding.UTF8.GetString(decryptedValue);

            return decryptedValueAsString;
        }

        public string EncryptAsymmetric(string txtToEncrypt)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(_publicKey);

            byte[] txtToEncryptAsByteArray = System.Text.Encoding.UTF8.GetBytes(txtToEncrypt);
            byte[] encryptedValue = rsa.Encrypt(txtToEncryptAsByteArray, true);

            string encryptedValueAsString = Convert.ToBase64String(encryptedValue);

            return encryptedValueAsString;
        }
        #endregion
    }
}
