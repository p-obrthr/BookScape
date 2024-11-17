using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepo;
    public BookService(IBookRepository bookRepository)
    {
        _bookRepo = bookRepository;
    }

    public async Task<ulong> AddBookAsync(Book book)
    => await _bookRepo.AddBookAsync(book);

    public async Task<List<Book>> GetAllBooksAsync()
    =>  await _bookRepo.GetAllBookAsync();

    public async Task<List<Book>> GetBooksByUserAsync(ulong id)
    => await _bookRepo.GetBooksByUserIdAsync(id);
}
