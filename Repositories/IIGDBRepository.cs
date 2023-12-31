using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesLibrary.Models;

namespace GamesLibrary.Repositories
{
    public interface IIGDBRepository
    {
        Task <IEnumerable<GameModel>> ReturnGamesAsync();
        Task <IEnumerable<GameModel>> SearchForGameAsync(string name);
    }
}