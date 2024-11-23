using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Application.Models;
using Application.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
public class BookController : ControllerBase
{
    private readonly IBookService _bookServ;
    public BookController(IBookService bookService)
    {   
        _bookServ = bookService;
    }

    [HttpGet("books")]
    [Authorize]
    public async Task<IActionResult> GetBooks()
    {
        var userId = GetId(User);
        if (userId == null) return Unauthorized("user id not found in token");
        
        var response = await _bookServ.GetBooksByUserAsync((int)userId);

        return Ok(response ?? new());
    }

    [HttpPost("book")]
    [Authorize]
    public async Task<IActionResult> Create(BookCreationRequest bookCreationReq)
    {
        var userId = GetId(User);
        if (userId == null) return Unauthorized("user id not found in token");

        var id = await _bookServ.AddBookAsync(bookCreationReq, (int)userId);
        return StatusCode(201, new { id,  bookCreationReq });
    }

    private int? GetId(ClaimsPrincipal user)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return null; 

        if (int.TryParse(userId, out int intValue)) return intValue; 
        
        return null; 
    }

}
