using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RotaProdutiva.Application.DTOs.Cursos;
using RotaProdutiva.Application.Interfaces;
using RotaProdutiva.Domain.Exceptions;

namespace RotaProdutiva.API.Controllers
{
    [ApiController]
    [Route("api/cursos")]
    public class CursosController : ControllerBase
    {
        private readonly ICursoService _cursoService;

        public CursosController(ICursoService cursoService)
        {
            _cursoService = cursoService;
        }

        private Guid UsuarioId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CursoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTodos()
        {
            var cursos = await _cursoService.ObterTodosAsync();
            return Ok(cursos);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CursoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            try
            {
                var curso = await _cursoService.ObterPorIdAsync(id);
                return Ok(curso);
            }
            catch (DomainException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Tutor,Admin")]
        [ProducesResponseType(typeof(CursoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Criar([FromBody] CriarCursoDto dto)
        {
            try
            {
                var curso = await _cursoService.CriarAsync(dto, UsuarioId);
                return CreatedAtAction(nameof(ObterPorId), new { id = curso.Id }, curso);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("tutor/{tutorId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<CursoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterPorTutor(Guid tutorId)
        {
            var cursos = await _cursoService.ObterPorTutorAsync(tutorId);
            return Ok(cursos);
        }
    }
}
