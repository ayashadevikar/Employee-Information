using Microsoft.Extensions.Options;
using MongoDB.Driver;
using FullStackCrud.Server.Models;
using FullStackCrud.Server.Data;
using System.Runtime;

namespace FullStackCrud.Server.Services
{
    public class EmployeeService
    {
        private readonly IMongoCollection<Employee> _employeeCollection;

        public EmployeeService(IOptions<DatabaseSettings> dbSettings)
        {
            Console.WriteLine($"EmployeeCollectionName: {dbSettings.Value.EmployeeCollectionName}");

            var mongoClient = new MongoClient(dbSettings.Value.Connection);
            var database = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
            _employeeCollection = database.GetCollection<Employee>(dbSettings.Value.EmployeeCollectionName);
        }

        public async Task<List<Employee>> GetAsync() =>
            await _employeeCollection.Find(_ => true).ToListAsync();

        public async Task<Employee?> GetAsync(string id) =>
            await _employeeCollection.Find(e => e.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Employee newEmployee) =>
            await _employeeCollection.InsertOneAsync(newEmployee);

        public async Task UpdateAsync(string id, Employee updatedEmployee) =>
            await _employeeCollection.ReplaceOneAsync(e => e.Id == id, updatedEmployee);

        public async Task DeleteAsync(string id) =>
            await _employeeCollection.DeleteOneAsync(e => e.Id == id);
    }
}
