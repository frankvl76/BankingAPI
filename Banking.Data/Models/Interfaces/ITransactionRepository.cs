using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Banking.Data.Models
{
    public interface ITransactionRepository
    {
        Task Add(Transaction transaction);
        Task AddRange(IEnumerable<Transaction> products);
        Task<Transaction> Find(Expression<Func<Transaction, bool>> filter);
        Task<IEnumerable<Transaction>> FindAll(Expression<Func<Transaction, bool>> filter);
        Task Remove(string key);
        Task Update(Transaction item);
    }
}