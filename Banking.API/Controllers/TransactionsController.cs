using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Banking.DTO.Transactions;
using Banking.BusinessServices.TransactionServices;
using System.Net;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Banking.API.Controllers
{
    /// <summary>
    /// Transaction controller version 1.0
    /// </summary>
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]")]
    [EnableCors("AllowAnyOrigin")]
    public class TransactionsController : Controller
    {
        private ITransactionService _transactionService;

        /// <summary>
        /// Transaction controller constructor
        /// </summary>        
        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Get all transactions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _transactionService.GetAllTransactions());
        }

        /// <summary>
        /// Get all transactions belonging to a transaction type
        /// </summary>
        /// <returns>List of transactions</returns>
        [HttpGet("TransactionTypes/{transactionType}")]
        public async Task<IActionResult> GetByTransactionType(string transactionType)
        {
            return Ok(await _transactionService.GetAllTransactionsByTransactionType(transactionType));
        }

        /// <summary>
        /// Get transaction by Id
        /// </summary>
        /// <param name="id">Id of transaction</param>
        /// <returns>Transaction information</returns>
        /// <response code="404">Not found</response>
        /// <response code="500">Server error</response>
        [HttpGet("{id}")]                
        public async Task<IActionResult> Get(int id)
        {
            var transaction = await _transactionService.GetTransaction(id);
            if (transaction != null)
                return Ok(transaction);
            else
                return NotFound();
        }

        /// <summary>
        /// Add transaction
        /// </summary>
        /// <param name="transaction">Transaction to be added</param>
        [HttpPost]
        public async Task Post([FromBody]TransactionCreateDTO transaction)
        {
            await _transactionService.AddTransaction(transaction);
        }

        ///// <summary>
        ///// Update transaction by Id
        ///// </summary>
        ///// <param name="id">Transaction Id</param>
        ///// <param name="value">Updated transaction information</param>
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]TransactionCreateDTO value)
        //{
        //}

        ///// <summary>
        ///// Delete transaction by Id
        ///// </summary>
        ///// <param name="id">Transaction Id</param>
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
