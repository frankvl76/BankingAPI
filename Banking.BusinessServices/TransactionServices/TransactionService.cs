using AutoMapper;
using Banking.Data.Models;
using Banking.DTO.Transactions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Banking.BusinessServices.TransactionServices
{
    public class TransactionService : ITransactionService
    {
        private ITransactionRepository _transactionRepository;
        private IMapper _mapper; 

        public TransactionService(ITransactionRepository transactionRepository,
                                  IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all transactions
        /// </summary>
        /// <returns>List of transactions</returns>
        public async Task<IEnumerable<TransactionCreateDTO>> GetAllTransactions()
        {
            var transactions = await _transactionRepository.FindAll(f => true);
            return _mapper.Map<List<TransactionCreateDTO>>(transactions); 
        }

        /// <summary>
        /// Get all transactions by transaction type
        /// </summary>
        /// <param name="transactionType">Current transaction type</param>
        /// <returns>List of transactions</returns>
        public async Task<IEnumerable<TransactionCreateDTO>> GetAllTransactionsByTransactionType(string transactionType)
        {
            var transactions = await _transactionRepository.FindAll(f => f.TypeOfTransaction == transactionType);
            return _mapper.Map<List<TransactionCreateDTO>>(transactions);
        }

        /// <summary>
        /// Get single transaction by Id
        /// </summary>
        /// <param name="id">Transaction Id</param>
        /// <returns>Transaction information (DTO)</returns>
        public async Task<TransactionCreateDTO> GetTransaction(int id)
        {
            //var transaction = await _transactionRepository.Find(t => t.Id == id);
            //return _mapper.Map<TransactionCreateDTO>(transaction);             
            return null;
        }

        /// <summary>
        /// Add new transactions, duplicates are filtered and will not be added
        /// </summary>
        /// <param name="transactionDTO">To be added transaction information</param>
        /// <returns>Nada</returns>
        public async Task AddTransaction(TransactionCreateDTO transactionDTO)
        {            
            if (await _transactionRepository.Find(t => t.Amount == transactionDTO.Amount &&
                                             t.Currency == transactionDTO.Currency &&
                                             t.DebitCredit == transactionDTO.DebitCredit &&
                                             t.Description == transactionDTO.Description &&
                                             t.MyAccount == transactionDTO.MyAccount &&
                                             t.ToAccount == transactionDTO.ToAccount &&
                                             t.TransactionDate_1 == transactionDTO.TransactionDate_1 &&
                                             t.TransactionDate_2 == transactionDTO.TransactionDate_2 &&
                                             t.TypeOfTransaction == transactionDTO.TypeOfTransaction) == null)
            {
                var transaction = _mapper.Map<Transaction>(transactionDTO);
                await _transactionRepository.Add(transaction);
            }
        }
    }
}
