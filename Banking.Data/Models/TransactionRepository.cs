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
    public class TransactionRepository : ITransactionRepository
    {
        private MongoDbSettings _settings;
        private IBankingToolsContext _context; 

        public TransactionRepository(IOptions<MongoDbSettings> settings,
                                     IBankingToolsContext context)
        {
            _settings = settings.Value;
            _context = context;
        }

        public async Task Add(Transaction transaction)
        {
            await _context.Set<Transaction>().InsertOneAsync(transaction);            
        }

        public async Task AddRange(IEnumerable<Transaction> products)
        {
            foreach (var product in products)
            {
                //await _context.Set<Transaction>().ReplaceOneAsync(c => c.Id == product.Id, product, new UpdateOptions() { IsUpsert = true });
            }            
        }

        public async Task<Transaction> Find(Expression<Func<Transaction, bool>> filter)
        {
            return await _context.Set<Transaction>().Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Transaction>> FindAll(Expression<Func<Transaction, bool>> filter)
        {
            var result = new List<Transaction>();
            result = await _context.Set<Transaction>().Find(filter).ToListAsync();
            return result;
        }

        public async Task Remove(string key)
        {
            var filter = Builders<Transaction>.Filter.Eq("Id", key);
            await _context.Set<Transaction>().DeleteOneAsync(filter);
        }

        public async Task Update(Transaction item)
        {
            //var filter = Builders<Transaction>.Filter.Eq("Id", item.Id);
            //await _context.Set<Transaction>().ReplaceOneAsync(filter, item, new UpdateOptions() { IsUpsert = true });
        }
    }
}
