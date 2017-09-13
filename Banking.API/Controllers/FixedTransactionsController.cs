using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Banking.BusinessServices.TransactionServices;
using Banking.DTO.Transactions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Banking.API.Controllers
{
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]")]
    [EnableCors("AllowAnyOrigin")]
    public class FixedTransactionsController : Controller
    {
        private IFixedTransactionService _fixedTransactionService;

        public FixedTransactionsController(IFixedTransactionService fixedTransactionService)
        {
            _fixedTransactionService = fixedTransactionService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _fixedTransactionService.GetFixedTransactions(DateTime.Now));
        }

        [HttpGet("FixedCharges")]
        public async Task<IActionResult> GetFixedCharges()
        {
            return Ok(await _fixedTransactionService.GetFixedCharges(DateTime.Now));
        }

        [HttpPost]
        public async Task Post([FromBody]FixedTransactionCreateDTO value)
        {
            await _fixedTransactionService.AddFixedTransaction(value);
        }

        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
