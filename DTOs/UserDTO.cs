using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesLibrary.DTOs
{
    public record UserDTO
    {
        public string? UserName {get; init;}
        public string? Email { get; init;}
        public string? Password { get; init; }
    }
}