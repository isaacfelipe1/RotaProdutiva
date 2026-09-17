using Microsoft.AspNetCore.Mvc;
using RotaProdutiva.Application.DTOs.Auth;
using RotaProdutiva.Application.Interfaces;
using RotaProdutiva.Domain.Exceptions;

namespace RotaProdutiva.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registrar")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registrar([FromBody] RegistrarUsuarioDto dto)
        {
            try
            {
                var resultado = await _authService.RegistrarAsync(dto);
                return Ok(resultado);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var resultado = await _authService.LoginAsync(dto);
                return Ok(resultado);
            }
            catch (DomainException ex)
            {
                return Unauthorized(new { mensagem = ex.Message });
            }
        }
    }
}
