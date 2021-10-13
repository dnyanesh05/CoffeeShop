using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShop.Models;
using MongoDB.Driver;

namespace CoffeeShop.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _orders = database.GetCollection<Order>("order");
        }

        public Order Create(Order order)
        {
            _orders.InsertOne(order);
            return order;
        }

        public IList<Order> Read() =>
            _orders.Find(sub => true).ToList();

        public Order Find(string id) =>
            _orders.Find(sub => sub.Id == id).SingleOrDefault();

        public void Update(Order order) =>
            _orders.ReplaceOne(ord => ord.Id == order.Id, order);

        public void Delete(string id) =>
            _orders.DeleteOne(sub => sub.Id == id);


        
    }
}
