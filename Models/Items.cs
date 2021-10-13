using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CoffeeShop.Models
{
    public class Items
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int itemId { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string categoryId { get; set; }
        public double unitPrice { get; set; }
        public bool isActive { get; set; }
        public DateTime createdOn { get; set; }
        public int createdBy { get; set; }
        public DateTime modifiedOn { get; set; }
        public int modifiedBy { get; set; }
    }
}
