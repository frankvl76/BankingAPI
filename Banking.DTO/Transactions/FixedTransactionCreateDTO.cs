using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.DTO.Transactions
{
    public class FixedTransactionCreateDTO
    {
        public string MyAccount { get; set; }
        public string ToAccount { get; set; }
        public decimal Amount { get; set; }
    }
}
