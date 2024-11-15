using Domain.Entities;

namespace Application.Interfaces;

public interface IBookRepository
{
    Task<Book> GetBookAsync(ulong id);
    Task<ulong> AddBookAsync(Book book);
    Task UpdateBookAsync(Book book);
    Task DeleteBookAsync(ulong id);
}
