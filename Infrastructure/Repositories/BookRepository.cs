using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using LinqToDB;

namespace Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<ulong> AddBookAsync(Book book)
    {
        var id = await _context.InsertWithIdentityAsync(book);
        return (ulong)(long)id;
    }

    public async Task DeleteBookAsync(ulong id)
    => await _context.DeleteAsync(id);

    public async Task<Book?> GetBookAsync(ulong id)
    { 
        var book =  await _context
                    .Books            
                    .FirstOrDefaultAsync(b => b.Id == id); 
        return book;
    }

    public async Task UpdateBookAsync(Book book)
    => await _context.UpdateAsync(book);

    public async Task<List<Book>> GetAllBookAsync()
    {
        var books = await _context.Books.ToListAsync(); 
        return books;
    }

    public async Task<List<Book>> GetBooksByUserIdAsync(ulong userId)
    {
        var books =  await _context
                    .Books            
                    .Where(b => b.UserId == userId)
                    .ToListAsync();
        return books;
    }
}
