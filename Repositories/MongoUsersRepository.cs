using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesLibrary.DTOs;
using GamesLibrary.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GamesLibrary.Repositories
{
    public class MongoUsersRepository : IUsersRepository
    {
        private const string databaseName = "myGames";
        private const string collectionName = "users";
        private readonly IMongoCollection<UserModel>? usersCollection;
        private readonly FilterDefinitionBuilder<UserModel> filterBuilder = Builders<UserModel>.Filter;

        public MongoUsersRepository(IMongoClient mongoClient) {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            usersCollection = database.GetCollection<UserModel>(collectionName);
        }

        public async Task AddGameToListAsync(UserModel user)
        {
            var filter = filterBuilder.Eq(existingUser => existingUser.Id,user.Id);
            await usersCollection?.ReplaceOneAsync(filter, user);
        }

        public async Task CreateUserAsync(UserModel user)
        {
            await usersCollection?.InsertOneAsync(user);
        }

        public async Task<UserModel> GetUserAsync(Guid id)
        {
            var filter = filterBuilder.Eq(user => user.Id, id);
            return await usersCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            return await usersCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<UserModel> SearchUserByEmailAsync(string email)
        {
            var filter = filterBuilder.Eq(user=> user.Email, email);
            return await usersCollection.Find(filter).SingleOrDefaultAsync();
        }

    }
}