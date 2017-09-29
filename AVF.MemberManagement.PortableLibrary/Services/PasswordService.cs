using System;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Org.BouncyCastle.Security;
using PCLCrypto;

namespace AVF.MemberManagement.PortableLibrary.Services
{
    public class PasswordService : IPasswordService
    {
        private const char Delimiter = ':';
        private const int SaltByteSize = 32;
        private const int Iterations = 10000;
        private const int KeyLengthInBytes = 20; //https://www.owasp.org/index.php/Using_Rfc2898DeriveBytes_for_PBKDF2 => With PBKDF2-SHA1 this is 160 bits or 20 bytes

        private string _hashAlgorithmForSecureRandom = "SHA512";

        readonly ISettingsProxy _settingsProxy;

        public PasswordService(ISettingsProxy settingsProxy)
        {
            _settingsProxy = settingsProxy;
        }

        public async Task<bool> IsValidAsync(string enteredPassword, string storedPasswordHash, string pepper)
        {
            var storedSalt = GetSalt(storedPasswordHash);

            var enterdPasswordHash = await HashPasswordAsync(enteredPassword, pepper, storedSalt);

            return enterdPasswordHash == storedPasswordHash;
        }

        private static byte[] GetSalt(string encryptedPassword)
        {
            var passwordParameters = encryptedPassword.Split(Delimiter);

            var storedSalt = passwordParameters[1];

            var saltBytes = Convert.FromBase64String(storedSalt);

            return saltBytes;
        }

        public async Task<string> HashPasswordAsync(string password, string pepper, byte[] saltBytes = null)
        {
            //string pepper = App.AppId;

            //TODO: Why is this throwing an execption sometimes when running the unit tests?
            _hashAlgorithmForSecureRandom = (await _settingsProxy.GetSettingAsync("HashAlgorithm")).Value;

            try
            {
                
            }
            catch
            {
                // ignored
            }

            if (saltBytes == null)
            {
                saltBytes = GenerateSecureRandomSalt();
            }

            //uses Pbkdf2
            var deriveBytes = NetFxCrypto.DeriveBytes.GetBytes(password + pepper, saltBytes, Iterations, KeyLengthInBytes);

            var hash = Convert.ToBase64String(deriveBytes);
            var salt = Convert.ToBase64String(saltBytes);
            var stringToStore = $"{hash}{Delimiter}{salt}";

            return stringToStore;
        }

        private byte[] GenerateSecureRandomSalt()
        {
            var saltBytes = new byte[SaltByteSize];
            var secureRandom = SecureRandom.GetInstance($"{_hashAlgorithmForSecureRandom}PRNG", true);
            secureRandom.NextBytes(saltBytes);
            return saltBytes;
        }
    }
}
