using MongoDB.Driver;

namespace mongoAPI.Models
{
    public class MongoApiDbContext {
        private readonly IMongoDatabase _itemsDb;
        public MongoApiDbContext ()
        {
            var client = new MongoClient("mongodb+srv://read_and_write:CWUhLNbNmTwyYZzE@cluster0-naxhs.mongodb.net/test?retryWrites=true&w=majority");
            _itemsDb = client.GetDatabase("Foods");
        }

        public IMongoCollection<Item> Item
        {
            get
            {
                return _itemsDb.GetCollection<Item>("SaladToppings");
            }
        }
    }
}
