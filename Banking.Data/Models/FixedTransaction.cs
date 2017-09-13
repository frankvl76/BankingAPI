using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Data.Models
{
    public class FixedTransaction
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string MyAccount { get; set; }
        public string ToAccount { get; set; }
        public decimal Amount { get; set; }
    }
}
