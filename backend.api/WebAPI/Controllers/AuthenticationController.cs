using Application.Interfaces.Authentication;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authServ;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authServ = authenticationService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> LogInUserAsync(LoginRequest loginRequest)
    {
        var result = await _authServ.LogInUserAsync(loginRequest);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> RegisterUserAsync(RegisterRequest registerRequest)
    {
        var result = await _authServ.RegisterUserAsync(registerRequest);
        return Ok(result);
    }
}
