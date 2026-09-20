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

        /// <summary>
        /// Obtém todos os cursos cadastrados.
        /// </summary>
        /// <returns>Lista de todos os cursos.</returns>
        /// <response code="200">Cursos retornados com sucesso.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CursoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTodos()
        {
            var cursos = await _cursoService.ObterTodosAsync();
            return Ok(cursos);
        }

        /// <summary>
        /// Obtém um curso pelo seu identificador.
        /// </summary>
        /// <param name="id">Identificador do curso.</param>
        /// <returns>Dados do curso encontrado.</returns>
        /// <response code="200">Curso retornado com sucesso.</response>
        /// <response code="404">Curso não encontrado.</response>
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

        /// <summary>
        /// Cria um novo curso.
        /// </summary>
        /// <param name="dto">Dados necessários para a criação do curso.</param>
        /// <returns>Dados do curso criado.</returns>
        /// <response code="201">Curso criado com sucesso.</response>
        /// <response code="400">Dados inválidos para criação do curso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário sem permissão para criar cursos.</response>
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

        /// <summary>
        /// Obtém todos os cursos cadastrados por um tutor específico.
        /// </summary>
        /// <param name="tutorId">Identificador do tutor.</param>
        /// <returns>Lista de cursos do tutor informado.</returns>
        /// <response code="200">Cursos retornados com sucesso.</response>
        [HttpGet("tutor/{tutorId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<CursoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterPorTutor(Guid tutorId)
        {
            var cursos = await _cursoService.ObterPorTutorAsync(tutorId);
            return Ok(cursos);
        }
    }
}
