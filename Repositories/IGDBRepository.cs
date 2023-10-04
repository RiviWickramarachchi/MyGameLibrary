using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesLibrary.Models;
using IGDB;
using IGDB.Models;

namespace GamesLibrary.Repositories
{

    public class IGDBRepository: IIGDBRepository
    {
        public async Task<IEnumerable<GamesLibrary.Models.GameModel>> ReturnGamesAsync() {
            List<GameModel> gamesList = new List<GameModel>();
            var igdb = new IGDBClient(
            // Found in Twitch Developer portal for your app
            "b6p5bloljnrbsbrc4nwqbd8q6w6n91",
            "yel60ypsl1ua11af3kdiv8xsdijcwe"
            );

            var games = await igdb.QueryAsync<Game>(
                IGDBClient.Endpoints.Games, query: "fields id,name,rating; where rating>75;sort rating desc;limit 10;"
            );
            foreach(var topgame in games) {
                GameModel game = new() {
                    GameID = topgame.Id.ToString(),
                    GameName = topgame.Name,
                    Rating = topgame.Rating
                };
                gamesList.Add(game);
            }
            return gamesList;
        }
    }
}