using Banking.Data.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Data.Models
{
    public class Repository<T> : IRepository<T>
    {
        private MongoDbSettings _settings;
        private IBankingToolsContext _context;

        public Repository(IOptions<MongoDbSettings> settings,
                          IBankingToolsContext context)
        {
            _settings = settings.Value;
            _context = context;
        }

        public async Task Add(T value)
        {
            await _context.Set<T>().InsertOneAsync(value);
        }

        public async Task AddRange(IEnumerable<T> products)
        {
            foreach (var product in products)
            {
                //await _context.Set<Transaction>().ReplaceOneAsync(c => c.Id == product.Id, product, new UpdateOptions() { IsUpsert = true });
            }
        }

        public async Task<T> Find(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> filter)
        {
            var result = new List<T>();
            result = await _context.Set<T>().Find(filter).ToListAsync();
            return result;
        }

        public async Task Remove(string key)
        {
            var filter = Builders<T>.Filter.Eq("Id", key);
            await _context.Set<T>().DeleteOneAsync(filter);
        }

        public async Task Update(T item)
        {
            //var filter = Builders<Transaction>.Filter.Eq("Id", item.Id);
            //await _context.Set<Transaction>().ReplaceOneAsync(filter, item, new UpdateOptions() { IsUpsert = true });
        }
    }
}
