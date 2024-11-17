using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<ulong> AddUserAsync(User user);
    Task<User?> GetUserByEmailAsync(string email);
}
