namespace GamesLibrary.DTOs
{
    public record UserDTO
    {
        public string? UserName {get; init;}
        public string? Email { get; init;}
        public string? Password { get; init; }
        public List<Models.GameModel>? Games { get; init; }
    }
}