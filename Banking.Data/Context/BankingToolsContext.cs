using Banking.Data.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Data.Models
{
    public class BankingToolsContext : IBankingToolsContext
    {
        private MongoDbSettings _mongoDbSettings { get; set; }
        private IMongoClient _client { get; set; }
        private IMongoDatabase _database { get; set; }
        private bool _disposed = false;

        public BankingToolsContext(IOptions<MongoDbSettings> mongoDbSettings)
        {
            _mongoDbSettings = mongoDbSettings.Value;
            _client = new MongoClient(_mongoDbSettings.Server);
            _database = _client.GetDatabase(_mongoDbSettings.Database);
        }

        public IMongoCollection<T> Set<T>()
        {
            return _database.GetCollection<T>(typeof(T).Name);
        }
    }
}
