using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using LinqToDB;
using LinqToDB.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    public UserRepository(AppDbContext appDbContext, IConfiguration configuration)
    { 
        _context = appDbContext;
        _configuration = configuration;
    }
    public async Task<LoginResponse> LoginUserAsync(LoginDTO loginDTO)
    {
        var user = await FindUserByEmailAsync(loginDTO.Email ?? "");
        if (user == null) return new (Flag: false, Message: "User not found");

        bool checkPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password);
        if(checkPassword)
        {
            string token = GenerateJWTToken(user);
            return new (true, "Login successfull", token);
        }
        else 
        {
            return new (false, "invalid credentials");
        }
    }

    private string GenerateJWTToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var userClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!)
        };
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: userClaims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials
        ); 
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<User?> FindUserByEmailAsync(string email)
    => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<RegistrationResponse> RegisterUserAsync(RegisterDTO registerDTO)
    {
        var getUser = await FindUserByEmailAsync(registerDTO.Email ?? "");
        if(getUser != null) return new(false, "user already exists");

        var user = new  User(){
            UserName = registerDTO.Name,
            Email = registerDTO.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password)
        };

        var id = await AddUserAsync(user);
        return new(true, "registration completed");
    }

    public async Task<ulong> AddUserAsync(User user)
    {
        var id = await _context.InsertWithIdentityAsync(user);
        return (ulong)(long)id;
    }

}
