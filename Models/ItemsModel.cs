using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace mongoAPI.Models 
{
    public class Item 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        [Required(ErrorMessage="Please give the item a name.")]
        public string Name { get; set; }
        [Required(ErrorMessage="Please give the item a category.")]
        public string Type { get; set; }
        [Required(ErrorMessage="Please give the item a valid price.")]
        public double price { get; set; }
    }
}