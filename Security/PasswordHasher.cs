using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace GamesLibrary.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 128 / 8;
        private const int KeySize = 256 / 8;
        private const int Iterations = 10000;
        private const char Delemiter = ';';
        private static readonly HashAlgorithmName _hashAlgorithmName =  HashAlgorithmName.SHA256;

        public string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password,salt,Iterations, _hashAlgorithmName,KeySize);

            return string.Join(Delemiter,Convert.ToBase64String(salt),Convert.ToBase64String(hash));
        }

        public bool Verify(string encryptedPassword, string passwordInput) {

            //Get Hash and Salt values in the encrypted password
            var elements = encryptedPassword.Split(Delemiter);
            var salt = Convert.FromBase64String(elements[0]);
            var hash = Convert.FromBase64String(elements[1]);

            var inputPwdHash = Rfc2898DeriveBytes.Pbkdf2(passwordInput,salt,Iterations,_hashAlgorithmName,KeySize);

            //Compare the hash values of the two input strings and return true if they match and false if they dont
            return CryptographicOperations.FixedTimeEquals(hash, inputPwdHash);
        }
    }
}