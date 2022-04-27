using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Services
{
    public interface IShortURLService
    {
        ShortUrl Get(string id);
        ShortUrl GetByPath(string path);
        ShortUrl Create(ShortUrl url);
        void Update(string id, ShortUrl url);
        void Remove(string id);
    }
}