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

        /// <summary>
        /// Registra um novo usuário no sistema.
        /// </summary>
        /// <param name="dto">Dados necessários para o cadastro do usuário.</param>
        /// <returns>Token de autenticação e informações do usuário registrado.</returns>
        /// <response code="200">Usuário registrado com sucesso.</response>
        /// <response code="400">Dados inválidos ou usuário já existente.</response>
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

        /// <summary>
        /// Realiza a autenticação de um usuário existente.
        /// </summary>
        /// <param name="dto">Credenciais de login (e-mail e senha).</param>
        /// <returns>Token de autenticação e informações do usuário autenticado.</returns>
        /// <response code="200">Login realizado com sucesso.</response>
        /// <response code="401">Credenciais inválidas.</response>
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
