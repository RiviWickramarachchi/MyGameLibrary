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
            int listSize = 10;
            int listCount = 0;
            List<GameModel> gamesList = new ();
            var igdb = new IGDBClient(

            igdb_client_id,
            igdb_client_id_secret

            );

            var games = await igdb.QueryAsync<Game>(
                IGDBClient.Endpoints.Games, query: "fields id,name,rating,summary, artworks.image_id; where rating>75;sort rating desc;limit 50;"
            );
            foreach(var topgame in games) {
                // The list is updated only if the image url is not null
                if(topgame.Artworks != null)
                {
                    var artworkImageId = topgame.Artworks.Values.First().ImageId;
                    string imgUrl = ImageHelper.GetImageUrl(imageId: artworkImageId, size: ImageSize.HD720, retina: false);
                    GameModel game = new() {
                        GameID = topgame.Id.ToString(),
                        GameName = topgame.Name,
                        Rating = topgame.Rating,
                        Description = topgame.Summary,
                        ImgUrl = imgUrl
                    };
                    gamesList.Add(game);
                    listCount++;
                    if(listCount == listSize)
                    {
                        listCount = 0;
                        break;
                    }
                }
            }
            return gamesList;
        }

        public async Task<IEnumerable<GameModel>> SearchForGameAsync(String name) {
            string gameName = name;
            gameName = gameName.Replace("'", "''"); // Escape special characters in the game name (e.g., single quotes)
            string igdb_client_id = _config["IGDB_CLIENT_ID"];
            string igdb_client_id_secret = _config["IGDB_CLIENT_SECRET"];

            // Create an empty Game model to return
            List<GameModel> gamesList = new ();
            var igdb = new IGDBClient(

            igdb_client_id,
            igdb_client_id_secret

            );

            string query = $"fields id,name,rating,summary, artworks.image_id; where name ~ *\"{gameName}\"*;";
            //string defaultImgUrl = "/Images/Covers/placeholder.jpg";
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query);
            //var game = games.FirstOrDefault();
            foreach(var game in games)
            {
                if(game.Artworks != null) {
                    var artworkImageId = game.Artworks.Values.First().ImageId;
                    string imgUrl = ImageHelper.GetImageUrl(imageId: artworkImageId, size: ImageSize.HD720, retina: false);
                    GameModel gameModel = new() {
                        ImgUrl = imgUrl,
                        GameID = game.Id.ToString(),
                        GameName = game.Name,
                        Description = game.Summary,
                        Rating = game.Rating
                    };
                    gamesList.Add(gameModel);
                }
            }
            return gamesList;
        }
    }
}