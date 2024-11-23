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

    public async Task<int> AddBookAsync(Book book)
    {
        var id = await _context.InsertWithIdentityAsync(book);
        return (int)(long)id;
    }

    public async Task DeleteBookAsync(int id)
    => await _context.DeleteAsync(id);

    public async Task<Book?> GetBookAsync(int id)
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

    public async Task<List<Book>> GetBooksByUserIdAsync(int userId)
    {
        var books =  await _context
                    .Books            
                    .Where(b => b.UserId == userId)
                    .ToListAsync();
        return books;
    }
}
