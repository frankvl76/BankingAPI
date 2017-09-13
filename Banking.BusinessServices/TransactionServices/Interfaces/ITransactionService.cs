using System.Collections.Generic;
using Banking.DTO.Transactions;
using System.Threading.Tasks;
using Banking.Data.Models;

namespace Banking.BusinessServices.TransactionServices
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionCreateDTO>> GetAllTransactions();
        Task<TransactionCreateDTO> GetTransaction(int id);
        Task AddTransaction(TransactionCreateDTO transactionDTO);
        Task<IEnumerable<TransactionCreateDTO>> GetAllTransactionsByTransactionType(string transactionType);
    }
}