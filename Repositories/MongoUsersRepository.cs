using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesLibrary.Models;
using MongoDB.Driver;

namespace GamesLibrary.Repositories
{
    public class MongoUsersRepository : IUsersRepository
    {
        private const string databaseName = "myGames";
        private const string collectionName = "users";
        private readonly IMongoCollection<UserModel>? usersCollection;

        public MongoUsersRepository(IMongoClient mongoClient) {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            usersCollection = database.GetCollection<UserModel>(collectionName);
        }

        public void CreateUser(UserModel user)
        {
            usersCollection?.InsertOne(user);
        }
    }
}