using Application.Models;

namespace Application.Interfaces.Authentication;

public interface IAuthenticationService
{
    Task<LoginResponse> LogInUserAsync(LoginRequest loginRequest);
    Task<RegistrationResponse> RegisterUserAsync(RegisterRequest registerRequest);
}
