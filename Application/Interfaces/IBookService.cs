using Application.Models;
using Domain.Entities;

namespace Application.Interfaces;

public interface IBookService
{
    Task<List<Book>> GetAllBooksAsync();
    Task<int> AddBookAsync(Book book);
    Task<int> AddBookAsync(BookCreationRequest bookCreationReq, int userId);
    Task<List<Book>> GetBooksByUserAsync(int userId);
}
