using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShop.Models;
using MongoDB.Driver;

namespace CoffeeShop.Services
{
    public class ItemsOrderService
    {
        private readonly IMongoCollection<ItemsOrder> _itemsOrder;

        public ItemsOrderService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _itemsOrder = database.GetCollection<ItemsOrder>("itemsOrder");
        }

        public ItemsOrder Create(ItemsOrder itemsOrder)
        {
            _itemsOrder.InsertOne(itemsOrder);
            return itemsOrder;
        }

        public IList<ItemsOrder> Read() =>
            _itemsOrder.Find(sub => true).ToList();

        public ItemsOrder Find(string id) =>
            _itemsOrder.Find(sub => sub.Id == id).SingleOrDefault();

        public void Update(ItemsOrder itemsOrder) =>
            _itemsOrder.ReplaceOne(ord => ord.Id == itemsOrder.Id, itemsOrder);

        public void Delete(string id) =>
            _itemsOrder.DeleteOne(sub => sub.Id == id);
    }
}
