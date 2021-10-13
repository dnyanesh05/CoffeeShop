using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShop.Models;
using MongoDB.Driver;

namespace CoffeeShop.Services
{
    public class ItemService
    {
        private readonly IMongoCollection<Items> _items;

        public ItemService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _items = database.GetCollection<Items>("items");
        }

        public Items Create(Items items)
        {
            _items.InsertOne(items);
            return items;
        }

        public IList<Items> Read() =>
            _items.Find(sub => true).ToList();

        public Items Find(string id) =>
            _items.Find(sub => sub.Id == id).SingleOrDefault();

        public void Update(Items items) =>
            _items.ReplaceOne(ord => ord.Id == items.Id, items);

        public void Delete(string id) =>
            _items.DeleteOne(sub => sub.Id == id);
    }
}
