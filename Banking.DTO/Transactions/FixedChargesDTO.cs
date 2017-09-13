using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.DTO.Transactions
{
    public class TransactionOverviewDTO
    {

        public ObjectId Id { get; set; }
        [JsonProperty("Recipient")]
        public string ToName { get; set; }
        [JsonProperty("Currency")]
        public string Currency { get; set; }
        [JsonProperty("Amount")]
        public decimal Amount { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("Date of transaction")]
        public string TransactionDate { get; set; }
        [JsonProperty("Paid")]
        public bool Paid { get; set; }
    }
}
