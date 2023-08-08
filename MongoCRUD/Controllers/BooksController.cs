using Mongo.CRUD.Models;
using Mongo.CRUD.Service;
using Microsoft.AspNetCore.Mvc;

namespace Mongo.CRUD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    protected readonly ILogger<BooksController> _logger;
    private readonly BooksService _booksService;

    public BooksController(BooksService booksService, ILogger<BooksController> logger)
    {
        _booksService = booksService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<List<Book>> Get()
    {
        try
        {
            var book = await _booksService.GetAsync();
            return book;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return new List<Book>();
        }
    }
        

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Book>> Get(string id)
    {
        var book = await _booksService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        return book;
    }

    [HttpPost]
    public async Task<IActionResult> Insert(Book newBook)
    {
        try
        {
            //System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions()
            //{
            //    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            //    WriteIndented = true,    
            //};

            ////var json = System.Text.Json.JsonSerializer.Serialize(fileFullPath, options);

            //var itemJson = System.IO.File.ReadAllText(fileFullPath);
            //var books = System.Text.Json.JsonSerializer.Deserialize<List<Book>>(itemJson);
            //var Result;
            //foreach (var book in books)
            //{
            //    await _booksService.CreateAsync(book);
            //    Result = CreatedAtAction(nameof(Get), new { id = book.Id }, book);
            //}

            await _booksService.CreateAsync(newBook);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
        }

        return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Book updatedBook)
    {
        var book = await _booksService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        updatedBook.Id = book.Id;

        await _booksService.UpdateAsync(id, updatedBook);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var book = await _booksService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        await _booksService.RemoveAsync(id);

        return NoContent();
    }
}