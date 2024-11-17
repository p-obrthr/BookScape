using Domain.Entities;

namespace Application.Interfaces.Authentication;

public interface IJwtGenerator
{
    string GenerateJWTToken(User user);
}
