using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesLibrary.Security
{
    public interface IPasswordHasher
    {
        public string HashPassword(string password);
    }
}