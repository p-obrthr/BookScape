using Application.Interfaces;
using Application.Models;
using Domain.Entities;

namespace Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepo;
    public BookService(IBookRepository bookRepository)
    {
        _bookRepo = bookRepository;
    }

    public async Task<int> AddBookAsync(Book book)
    => await _bookRepo.AddBookAsync(book);

    public async Task<int> AddBookAsync(BookCreationRequest bookCreationReq, int userId)
    {
         var  book = new Book()
        {
            UserId = userId,
            Title = bookCreationReq.Title,
            Author = bookCreationReq.Author,
            IsCompleted = false
        };
        return await AddBookAsync(book);
    }

    public async Task<List<Book>> GetAllBooksAsync()
    =>  await _bookRepo.GetAllBookAsync();

    public async Task<List<Book>> GetBooksByUserAsync(int id)
    => await _bookRepo.GetBooksByUserIdAsync(id);
}
