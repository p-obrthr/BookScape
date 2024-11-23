using Application.Interfaces.Authentication;

namespace Infrastructure.Services;

public class Hasher : IHasher
{
    public string HashPassword(string password)
    => BCrypt.Net.BCrypt.HashPassword(password);

    public bool VerifyPassword(string requestedPw, string userPw)
    => BCrypt.Net.BCrypt.Verify(requestedPw, userPw);
}
