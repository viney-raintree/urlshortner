using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace API.Models
{
    [BsonIgnoreExtraElements]
    public class ShortUrl
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonElement("expirytime")]
        public DateTime ExpiryTime {get; set;}
        
        [BsonElement("originalurl")]
        public string OriginalUrl { get; set; } = String.Empty;
        
        [BsonElement("nanoid")]
        public string Nanoid { get; set; } = String.Empty;

        [BsonElement("subdomain")]
        public string Subdomain {get; set;} = String.Empty;
        
        [BsonElement("cacheinmins")]
        public int CacheInMins {get; set;}
    }
}
