namespace Application.Interfaces.Authentication;

public interface IHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string dtoPw, string userPw);
}
