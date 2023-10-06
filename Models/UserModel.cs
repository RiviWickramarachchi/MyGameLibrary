using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesLibrary.Models
{
    public class UserModel
    {
    #nullable disable
        public Guid Id { get; init;}
        public string UserName {get; init;}
        public string Email { get; init;}
        public string EncryptedPassword { get; init; }
    #nullable enable
        public DateTimeOffset CreatedDate { get; init;}
        public List<GameModel>? Games { get; init; }
    }
}