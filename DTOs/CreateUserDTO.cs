using System.ComponentModel.DataAnnotations;

namespace GamesLibrary.DTOs
{
    public record CreateUserDTO
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

    }
}