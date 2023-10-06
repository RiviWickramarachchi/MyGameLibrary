using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GamesLibrary.DTOs
{
    public record LoginDTO
    {
    #nullable disable
        [Required]
        public string Email { get; init;}
        [Required]
        public string Password { get; init; }
    #nullable enable
    }
}