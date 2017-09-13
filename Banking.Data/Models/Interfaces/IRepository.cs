using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Banking.Data.Models
{
    public interface IRepository<T>
    {
        Task Add(T value);
        Task AddRange(IEnumerable<T> products);
        Task<T> Find(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> filter);
        Task Remove(string key);
        Task Update(T item);
    }
}