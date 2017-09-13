using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Data.Models
{
    public class Transaction
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string MyAccount { get; set; }
        public string ToAccount { get; set; }
        public string Currency { get; set; }
        public string DebitCredit { get; set; }
        public string ToName { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string TypeOfTransaction { get; set; }
        public DateTime TransactionDate_1 { get; set; }
        public DateTime TransactionDate_2 { get; set; }
    }
}
