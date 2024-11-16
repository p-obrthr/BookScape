using Application.DTOs;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<RegistrationResponse> RegisterUserAsync(RegisterDTO registerDTO);
    Task<LoginResponse> LoginUserAsync(LoginDTO loginDTO);
}
