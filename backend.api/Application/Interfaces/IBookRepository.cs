using Domain.Entities;

namespace Application.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetBookAsync(int id);
    Task<int> AddBookAsync(Book book);
    Task UpdateBookAsync(Book book);
    Task DeleteBookAsync(int id);
    Task<List<Book>> GetAllBookAsync();
    Task<List<Book>> GetBooksByUserIdAsync(int id);
}
