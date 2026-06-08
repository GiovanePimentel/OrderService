using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{

    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("/auth/token")]
    public async Task<IActionResult> Token([FromBody] LoginRequest request)
    {
        if (request.Username != "admin" || request.Password != "admin")
        {
            return Unauthorized(new
            {
                message = "Invalid credentials"
            });
        }
        var jwt = await _authService.GenerateToken(request);
        return Ok(new TokenResponse
        {
            Token = jwt.ToString()
        });
    }
}