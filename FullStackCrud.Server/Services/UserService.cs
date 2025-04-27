using FullStackCrud.Server.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using FullStackCrud.Server.Data;


namespace FullStackCrud.Server.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserService(IOptions<DatabaseSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.Connection);
            var database = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
            _usersCollection = database.GetCollection<User>(dbSettings.Value.UserCollectionName);
        }

        public async Task<User> GetUserAsync(string username) =>
           await _usersCollection.Find(u => u.Username == username).FirstOrDefaultAsync();

        // Get user by email (Add this method)
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _usersCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
        }
        // Async CreateUser
        public async Task CreateUserAsync(User user) =>
            await _usersCollection.InsertOneAsync(user);

        //public void CreateUser(User user) =>
        //    _usersCollection.InsertOne(user);
    }
}
