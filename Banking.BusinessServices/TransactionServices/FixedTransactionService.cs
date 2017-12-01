using AutoMapper;
using Banking.Data.Models;
using Banking.DTO.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.BusinessServices.TransactionServices
{
    public class FixedTransactionService : IFixedTransactionService
    {
        private ITransactionRepository _transactionRepository;
        private IMapper _mapper;
        private IRepository<FixedTransaction> _fixedRepository;

        public FixedTransactionService(ITransactionRepository transactionRepository,
                                       IMapper mapper,
                                       IRepository<FixedTransaction> fixedRepository)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _fixedRepository = fixedRepository;
        }

        /// <summary>
        /// Add a transaction to the fixed-transaction repository
        /// </summary>
        /// <param name="transaction">DTO containing fixed-transaction information</param>
        /// <returns>nada</returns>
        public async Task AddFixedTransaction(FixedTransactionCreateDTO transaction)
        {
            var fixedTransaction = await _fixedRepository.Find(t => t.MyAccount == transaction.MyAccount
                                                                 && t.ToAccount == transaction.ToAccount);
                                                                 //&& transaction.Amount > 0 ? t.Amount == transaction.Amount : true);
            if (fixedTransaction == null)
                await _fixedRepository.Add(_mapper.Map<FixedTransaction>(transaction));            
        }

        /// <summary>
        /// Get fixed transactions from list of transactions based on fixed transactions stam table
        /// </summary>
        /// <param name="currentDate">DateTime object containing month</param>
        /// <returns>List of fixed transactions</returns>
        public async Task<IEnumerable<TransactionOverviewDTO>> GetFixedTransactions(DateTime currentDate, string accountNumber)
        {
            var result = new List<Transaction>();
            IEnumerable<Transaction> transactions;
            
            if (string.IsNullOrEmpty(accountNumber))
                transactions = await _transactionRepository.FindAll(t => t.DebitCredit.ToUpper() == "D"
                                                                      && t.TransactionDate_1 > DateTime.Now.AddDays(-90));
            else
                transactions = await _transactionRepository.FindAll(t => t.MyAccount == accountNumber
                                                                      && t.DebitCredit.ToUpper() == "D"
                                                                      && t.TransactionDate_1 > DateTime.Now.AddDays(-90));

            IEnumerable<FixedTransaction> fixedTransactions;
            if (string.IsNullOrEmpty(accountNumber))
                fixedTransactions = await _fixedRepository.FindAll(t => true);
            else
                fixedTransactions = await _fixedRepository.FindAll(t => t.MyAccount == accountNumber);

            // Add all transactions that are fixed transactions
            foreach (var transaction in transactions)
            {
                if (fixedTransactions.Any(t => t.MyAccount == transaction.MyAccount && t.ToAccount == transaction.ToAccount))
                    result.Add(transaction);                
            }

            // Add all db-transactions 
            IEnumerable<Transaction> dbTransactions;
            if (string.IsNullOrEmpty(accountNumber))
                dbTransactions = await _transactionRepository.FindAll(t => t.DebitCredit.ToUpper() == "D"
                                                                      && (t.TypeOfTransaction.ToLower() == "db" || t.TypeOfTransaction.ToLower() == "dv")
                                                                      && t.TransactionDate_1 > DateTime.Now.AddDays(-90));
            else
                dbTransactions = await _transactionRepository.FindAll(t => t.DebitCredit.ToUpper() == "D"
                                                                      && t.MyAccount == accountNumber
                                                                      && (t.TypeOfTransaction.ToLower() == "db" || t.TypeOfTransaction.ToLower() == "dv")
                                                                      && t.TransactionDate_1 > DateTime.Now.AddDays(-90));

            result.AddRange(dbTransactions);

            // filter out doubles
            var res = from element in result
                      group element by element.ToAccount
                      into groups
                      where groups.Count() > 1
                      select groups.OrderBy(p => p.TransactionDate_1).Last();

            var res2 = res.Where(t => t.TransactionDate_1 > DateTime.Now.AddDays(-90))
                            .ToList().OrderBy(r => r.TransactionDate_1);

            // return DTO result
            return _mapper.Map<List<TransactionOverviewDTO>>(res2);
        }

        /// <summary>
        /// Calculate the fixed charges for a month based on currentDate
        /// </summary>
        /// <param name="currentDate">DateTime object containing month</param>
        /// <returns>List of fixed charges</returns>
        public async Task<IEnumerable<TransactionOverviewDTO>> GetFixedCharges(DateTime currentDate)
        {
            var transactions = await _transactionRepository.FindAll(t => (t.TypeOfTransaction == "ei"  || t.TypeOfTransaction == "db" || t.TypeOfTransaction == "IC")
                                                                    && !t.ToName.ToLower().Contains("paypal")
                                                                    && t.DebitCredit.ToUpper() == "D"
                                                                    && !t.ToName.ToLower().Contains("tls")                                                                    
                                                                    );

            var res = from element in transactions
                      group element by element.ToAccount
                      into groups
                      where groups.Count() > 1
                      select groups.OrderBy(p => p.TransactionDate_1).Last();

            var result = res.Where(t => t.TransactionDate_1 > DateTime.Now.AddDays(-90))
                            .ToList().OrderBy(r => r.TransactionDate_1);

            return _mapper.Map<List<TransactionOverviewDTO>>(result);
        }
    }
}
