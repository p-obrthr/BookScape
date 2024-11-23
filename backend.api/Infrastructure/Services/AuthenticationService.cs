using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Models;
using Domain.Entities;

namespace Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepo;
    private readonly IJwtGenerator _jwtGen;
    private readonly IHasher _hasher;
    public AuthenticationService(IUserRepository userRepository, IJwtGenerator jwtGenerator, IHasher hasher)
    {
        _userRepo = userRepository;
        _jwtGen = jwtGenerator;
        _hasher = hasher;
    }
    public async Task<LoginResponse> LogInUserAsync(LoginRequest loginRequest)
    {

        var user = await _userRepo.GetUserByEmailAsync(loginRequest.Email ?? "");
        if (user == null) return new (Flag: false, Message: "User not found");

        bool checkPassword = _hasher.VerifyPassword(loginRequest.Password!, user.Password!);
        if(checkPassword)
        {
            string token = _jwtGen.GenerateJWTToken(user);
            return new (true, "Login successfull", token);
        }
        else 
        {
            return new (false, "invalid credentials");
        }
    }

    public async Task<RegistrationResponse> RegisterUserAsync(RegisterRequest registerRequest)
    {
        var user = await _userRepo.GetUserByEmailAsync(registerRequest.Email ?? "");
        if(user != null) return new(false, "user already exists");

        user = new User(){
            UserName = registerRequest.Name,
            Email = registerRequest.Email,
            Password = _hasher.HashPassword(registerRequest.Password!)
        };

        var id = await _userRepo.AddUserAsync(user);
        return new(true, "registration completed");
    }
}