using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using Nanoid;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShortUrlsController : Controller
    {
        private readonly IShortURLService _shortURLService;
        private readonly IServer _server;


        public ShortUrlsController(IShortURLService shortURLService,IServer server)
        {
            _shortURLService = shortURLService;
            _server = server;
        }

      [HttpGet("{id}")]
        public ActionResult<ShortUrl> Get(string id)
        {
            var surl = _shortURLService.Get(id);

            if (surl == null)
            {
                return NotFound($"Short URL with Id = {id} not found");
            }

            return surl;
        }

       // POST api/<ShortUrlsController>
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ShortUrl shortUrl)
        {
            TryValidateModel(shortUrl);
            if (ModelState.IsValid)
            {
                var Nid = Nanoid.Nanoid.Generate(size:10);
                var shortenedURL = "";
                var addresses = _server.Features.Get<IServerAddressesFeature>().Addresses;
                string hosturl = addresses.ToArray<String>()[0];
                if (shortUrl.Subdomain == ""){
                    shortenedURL = string.Concat(hosturl, "/",Nid);
                }
                else shortenedURL = MakeShortUrl(hosturl,shortUrl.Subdomain,Nid);
                Console.WriteLine($"Shortened url: {shortenedURL}");                
                shortUrl.Nanoid = Nid;
                shortUrl.ShortenedURL = shortenedURL;
                DateTime date = DateTime.Now;
                int res = DateTime.Compare(shortUrl.ExpiryTime, date);
                if (res < 0) {
                    DateTime expireDate = date.AddDays(7);
                    shortUrl.ExpiryTime = expireDate;
                }
                Console.WriteLine($"Date time comparison result: {res}");                

                _shortURLService.Create(shortUrl);
                return CreatedAtAction(actionName: nameof(Get), new { id = shortUrl.Id, shortenedURL = shortenedURL}, shortUrl);
                //return Content(shortenedURL);
            }
            return NotFound();
        }

        private static string MakeShortUrl(string host, string subdomain, string Nid)
        {
            string delim = "";
            string shortenedURL = "";
            if (host.IndexOf("www") < 0)
            {
                delim = "//";
            }
            else
            {
                delim = "//www.";
            }
            string[] tokens = host.Split(new[] { delim }, StringSplitOptions.None);

            if (tokens.Length > 1)
            {
                shortenedURL = string.Concat(tokens[0], delim, subdomain, ".", tokens[1], "/", Nid);
            }

            return shortenedURL;
        }

        [HttpGet("/{nanoid:required}", Name = "ShortUrls_RedirectTo")]
        public async Task<IActionResult> RedirectTo(string nanoid)
        {
            Console.WriteLine($"inside get, need to redirect {nanoid}");
            if (nanoid == null) 
            {
                return NotFound();
            }

            var shortUrlobj = _shortURLService.GetByNanoid(nanoid);
            if (shortUrlobj == null) 
            {
                return NotFound();
            }

            Console.WriteLine($"inside get, found url to redirect {shortUrlobj.OriginalUrl}");
            return RedirectPermanent(shortUrlobj.OriginalUrl);
        }


    // PUT api/<ShortUrlsController>/id
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(string id, [FromBody] ShortUrl surl)
    {
        var urlobj = _shortURLService.Get(id);

        if (urlobj == null)
        {
            return NotFound($"URL with Id = {id} not found");
        }

        _shortURLService.Update(id, surl);

        return NoContent();
    }

    // DELETE api/<ShortUrlsController>/id
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var urlobj = _shortURLService.Get(id);

        if (urlobj == null)
        {
            return NotFound($"URL with Id = {id} not found");
        }

        _shortURLService.Remove(urlobj.Id);

        return Ok($"URL with Id = {id} deleted");
    }


    }
}
