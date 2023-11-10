using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesLibrary.DTOs;
using GamesLibrary.Models;

namespace GamesLibrary.Repositories
{
    public interface IUsersRepository
    {
        //Register User - Add the user to the database
        Task CreateUserAsync(UserModel user);

         //add game to list
        Task AddGameToListAsync(UserModel user);

        //Get User Info
        Task<UserModel> GetUserAsync(Guid id);

        //Get All Users
        Task<IEnumerable<UserModel>> GetUsersAsync();

        //Get MyGames - Returns the Games in user Library

        //Get User by email
        Task<UserModel> SearchUserByEmailAsync(string email);

    }
}