using Domain.Entities;

namespace Domain.Interfaces;

public interface IBookService
{
    Task<List<Book>> GetAllBooksAsync();
    Task<ulong> AddBookAsync(Book book);
}
