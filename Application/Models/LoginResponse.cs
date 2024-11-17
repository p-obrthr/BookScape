namespace Application.Models;

public record LoginResponse(bool Flag, string Message = null!, string Token = null!);

