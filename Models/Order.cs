using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CoffeeShop.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int orderId { get; set; }
        public double cost { get; set; }
        public int StatusId { get; set; }
        public string status { get; set; }
        public DateTime createdOn { get; set; }
        public int createdBy { get; set; }
        public DateTime modifiedOn { get; set; }
        public int modifiedBy { get; set; }
        public List<ItemsOrder> ItemsOrder { get; set; }
    }

    public enum Status
    {
        open,
        inprogress,
        ready,
        delivered
    }
}
