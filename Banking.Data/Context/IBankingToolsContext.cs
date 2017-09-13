using MongoDB.Driver;

namespace Banking.Data.Models
{
    public interface IBankingToolsContext
    {
        IMongoCollection<T> Set<T>();
    }
}