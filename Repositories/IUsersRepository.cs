using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesLibrary.Models;

namespace GamesLibrary.Repositories
{
    public interface IUsersRepository
    {
        //Register User - Add the user to the database
        void CreateUser(UserModel user);
        //Get MyGames - Returns the Games in user Library
    }
}