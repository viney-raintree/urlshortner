using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using API.Data;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using Nanoid;

/*
[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly IStudentService studentService;

    public StudentsController(IStudentService studentService)
    {
        this.studentService = studentService;
    }
    // GET: api/<StudentsController>
    [HttpGet]
    public ActionResult<List<Student>> Get()
    {
        return studentService.Get();
    }

    // GET api/<StudentsController>/5
    [HttpGet("{id}")]
    public ActionResult<Student> Get(string id)
    {
        var student = studentService.Get(id);

        if (student == null)
        {
            return NotFound($"Student with Id = {id} not found");
        }

        return student;
    }

    // POST api/<StudentsController>
    [HttpPost]
    public ActionResult<Student> Post([FromBody] Student student)
    {
        studentService.Create(student);

        return CreatedAtAction(nameof(Get), new { id = student.Id }, student);
    }

    // PUT api/<StudentsController>/5
    [HttpPut("{id}")]
    public ActionResult Put(string id, [FromBody] Student student)
    {
        var existingStudent = studentService.Get(id);

        if (existingStudent == null)
        {
            return NotFound($"Student with Id = {id} not found");
        }

        studentService.Update(id, student);

        return NoContent();
    }

    // DELETE api/<StudentsController>/5
    [HttpDelete("{id}")]
    public ActionResult Delete(string id)
    {
        var student = studentService.Get(id);

        if (student == null)
        {
            return NotFound($"Student with Id = {id} not found");
        }

        studentService.Remove(student.Id);

        return Ok($"Student with Id = {id} deleted");
    }
}
*/
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


       // POST api/<ShortUrlsController>
        [HttpPost]
        public ActionResult Create([FromBody] ShortUrl shortUrl)
//        public async Task<ActionResult> Create([FromBody] ShortUrl shortUrl)
        {
            //TryValidateModel(shortUrl);
            Console.WriteLine("outside validate");
            //var Nanoid = "";
           // if (ModelState.IsValid)
            //{
                var Nid = Nanoid.Nanoid.Generate(size:10);
                Console.WriteLine("after validate",Nid);
                _shortURLService.Create(shortUrl);
                //return CreatedAtAction(nameof(RedirectTo), new { id = shortUrl.Id}, shortUrl);

                return Content(Nid);
                //return RedirectToAction(actionName: nameof(Show), routeValues: new { id = shortUrl.Id, nanoid = shortUrl.Nanoid });
            //}
            //return NotFound();
            //return View(shortUrl);
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
    }
}
