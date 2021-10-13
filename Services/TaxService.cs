using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShop.Models;
using MongoDB.Driver;

namespace CoffeeShop.Services
{
    public class TaxService
    {
        private readonly IMongoCollection<Tax> _tax;

        public TaxService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _tax = database.GetCollection<Tax>("tax");
        }

        public Tax Create(Tax tax)
        {
            _tax.InsertOne(tax);
            return tax;
        }

        public IList<Tax> Read() =>
            _tax.Find(sub => true).ToList();

        public Tax Find(string id) =>
            _tax.Find(sub => sub.categoryId == id).SingleOrDefault();

        public void Update(Tax tax) =>
            _tax.ReplaceOne(ord => ord.Id == tax.Id, tax);

        public void Delete(string id) =>
            _tax.DeleteOne(sub => sub.Id == id);
    }
}
