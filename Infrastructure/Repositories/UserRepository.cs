using Domain.Entities;
using Infrastructure.Data;
using LinqToDB;
using Application.Interfaces;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext appDbContext)
    { 
        _context = appDbContext;
    }

    public async Task<ulong> AddUserAsync(User user)
    {
        var id = await _context.InsertWithIdentityAsync(user);
        return (ulong)(long)id;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
}
