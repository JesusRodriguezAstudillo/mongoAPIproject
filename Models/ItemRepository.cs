using System;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mongoAPI.Models
{
    public class ItemRepository : IItemRepository 
    {
        MongoApiDbContext db = new MongoApiDbContext();

        public async Task Add(Item item)
        {
            try
            {
                await db.Item.InsertOneAsync(item);
            }
            catch
            {
                throw;
            }
        }

        public async Task Update(Item item)
        {
            try
            {
                await db.Item.ReplaceOneAsync(filter: g => g._id == item._id, replacement: item);
            }
            catch
            {
                throw;
            }
        }

        public async Task Delete(string id)
        {
            try
            {
                FilterDefinition<Item> itemData = Builders<Item>.Filter.Eq("_id", id);
                await db.Item.DeleteOneAsync(itemData);               
            }
            catch
            {
                throw;
            }
        }

        public async Task<Item> GetItem(string id) 
        {
            try
            {
                FilterDefinition<Item> filter = Builders<Item>.Filter.Eq("_id", id);
                return await db.Item.Find(filter).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Item> GetItem(string name, bool byName)
        {
            try
            {
                FilterDefinition<Item> filter = Builders<Item>.Filter.Eq("Name", name);
                return await db.Item.Find(filter).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Item>> GetItems()
        {
            try
            {
                return await db.Item.Find(_ => true).ToListAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}