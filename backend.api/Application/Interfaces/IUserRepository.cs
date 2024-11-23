using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<int> AddUserAsync(User user);
    Task<User?> GetUserByEmailAsync(string email);
}
