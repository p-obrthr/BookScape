using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepo;

    public UserController(IUserRepository userRepository)
    {
        _userRepo = userRepository;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> LogUserIn(LoginDTO loginDTO)
    {
        var result = await _userRepo.LoginUserAsync(loginDTO);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> RegisterUser(RegisterDTO registerDTO)
    {
        var result = await _userRepo.RegisterUserAsync(registerDTO);
        return Ok(result);
    }
}
