using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Services;
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

        public ShortUrlsController(IShortURLService shortURLService)
        {
            _shortURLService = shortURLService;
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
//        public ActionResult Create([FromBody] ShortUrl shortUrl)
        public async Task<ActionResult> Create([FromBody] ShortUrl shortUrl)
        {
            TryValidateModel(shortUrl);
            if (ModelState.IsValid)
            {
                var Nid = Nanoid.Nanoid.Generate(size:10);
                Console.WriteLine(Nid);
                _shortURLService.Create(shortUrl);
                //return CreatedAtAction(actionName: nameof(Get), new { id = shortUrl.Id, nanoid = Nid}, shortUrl);
                return Content(Nid);
            }
            return NotFound();
        }

        // public IActionResult Show(int? id)
        // {
        //     if (!id.HasValue) 
        //     {
        //         return NotFound();
        //     }

        //     var shortUrl = _shortURLService.GetById(id.Value);
        //     if (shortUrl == null) 
        //     {
        //         return NotFound();
        //     }

        //     ViewData["Path"] = ShortUrlHelper.Encode(shortUrl.Id);

        //     return View(shortUrl);
        // }

        [HttpGet("/ShortUrls/RedirectTo/{path:required}", Name = "ShortUrls_RedirectTo")]
        public IActionResult RedirectTo(string path)
        {
            if (path == null) 
            {
                return NotFound();
            }

            var shortUrl = _shortURLService.GetByPath(path);
            if (shortUrl == null) 
            {
                return NotFound();
            }

            return Redirect(shortUrl.OriginalUrl);
        }

    // PUT api/<ShortUrlsController>/id
    [HttpPut("{id}")]
    public ActionResult Put(string id, [FromBody] ShortUrl surl)
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
    public ActionResult Delete(string id)
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
