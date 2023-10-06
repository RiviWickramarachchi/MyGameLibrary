using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesLibrary.Models;
using IGDB;
using IGDB.Models;
using MongoDB.Driver;

namespace GamesLibrary.Repositories
{

    public class IGDBRepository: IIGDBRepository
    {
        private readonly IConfiguration _config;

        public IGDBRepository(IConfiguration config) {
            _config = config;
        }
        public async Task<IEnumerable<GameModel>> ReturnGamesAsync() {
            // The generated Client id and Client Secret are saved in the secrets.json file
            //These keys can be generated and seen in the Twitch Developer portal for your app
            string igdb_client_id = _config["IGDB_CLIENT_ID"];
            string igdb_client_id_secret = _config["IGDB_CLIENT_SECRET"];
            List<GameModel> gamesList = new();
            var igdb = new IGDBClient(

            igdb_client_id,
            igdb_client_id_secret

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