using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesLibrary.DTOs;
using GamesLibrary.Models;

namespace GamesLibrary
{
    public static class Extensions
    {
        public static UserDTO ReturnAsDTO(this UserModel user) {
            return new UserDTO() {
                UserName = user.UserName,
                Email = user.Email,
                Password = user.EncryptedPassword,
                Games = user.Games
            };
        }
    }
}