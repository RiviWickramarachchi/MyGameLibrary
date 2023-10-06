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
    }
}