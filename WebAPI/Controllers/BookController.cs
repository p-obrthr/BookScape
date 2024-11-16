using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IBookService _bookServ;
    public BookController(IBookService bookService)
    {   
        _bookServ = bookService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var response = await _bookServ.GetAllBooksAsync();
        return Ok(response);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(Book book)
    {
        var id = await _bookServ.AddBookAsync(book);
        return StatusCode(201, new { id,  book });
    }
}