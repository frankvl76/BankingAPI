using System;
using System.Collections.Generic;
using Banking.DTO.Transactions;
using System.Threading.Tasks;

namespace Banking.BusinessServices.TransactionServices
{
    public interface IFixedTransactionService
    {
        Task<IEnumerable<TransactionOverviewDTO>> GetFixedTransactions(DateTime currentDate, string accountNumber);
        Task<IEnumerable<TransactionOverviewDTO>> GetFixedCharges(DateTime currentDate);
        Task AddFixedTransaction(FixedTransactionCreateDTO transaction);
    }
}