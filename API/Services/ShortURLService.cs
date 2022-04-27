using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using MongoDB.Driver;

namespace API.Services
{
    public class ShortURLService : IShortURLService
    {
        private readonly IMongoCollection<ShortUrl> _shorturls;

        public ShortURLService(IShortURLDatabaseSettings settings, IMongoClient mongoClient)
        {
           IMongoDatabase database = mongoClient.GetDatabase(settings.DatabaseName);
           _shorturls = database.GetCollection<ShortUrl>(settings.ShortURLCollectionName);
        }
        
        public ShortUrl Create(ShortUrl shorturl)
        {
            _shorturls.InsertOne(shorturl);
            return shorturl;
        }

        public ShortUrl Get(string id)
        {
            return _shorturls.Find(shorturl => shorturl.Id == id).FirstOrDefault();
        }

        public ShortUrl GetByPath(string path)
        {
            return _shorturls.Find(shorturl => shorturl.Nanoid == path).FirstOrDefault();
        }

        public void Remove(string id)
        {
            _shorturls.DeleteOne(ShortUrl => ShortUrl.Id == id);
        }

        public void Update(string id, ShortUrl shorturl)
        {
            _shorturls.ReplaceOne(ShortUrl => ShortUrl.Id == id, shorturl);
        }
    }
}