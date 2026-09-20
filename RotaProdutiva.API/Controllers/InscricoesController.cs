using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RotaProdutiva.Application.DTOs.Inscricoes;
using RotaProdutiva.Application.Interfaces;
using RotaProdutiva.Domain.Exceptions;

namespace RotaProdutiva.API.Controllers
{
    [ApiController]
    [Route("api/inscricoes")]
    public class InscricoesController : ControllerBase
    {
        private readonly IInscricaoService _inscricaoService;

        public InscricoesController(IInscricaoService inscricaoService)
        {
            _inscricaoService = inscricaoService;
        }

        /// <summary>
        /// Cria uma nova inscrição de um aluno em um curso.
        /// </summary>
        /// <param name="dto">Dados necessários para realizar a inscrição.</param>
        /// <returns>Dados da inscrição criada.</returns>
        /// <response code="200">Inscrição criada com sucesso.</response>
        /// <response code="400">Dados inválidos ou inscrição já existente.</response>
        [HttpPost]
        [ProducesResponseType(typeof(InscricaoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Criar([FromBody] CriarInscricaoDto dto)
        {
            try
            {
                var inscricao = await _inscricaoService.CriarAsync(dto);
                return Ok(inscricao);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Obtém todas as inscrições de um curso específico.
        /// </summary>
        /// <param name="cursoId">Identificador do curso.</param>
        /// <returns>Lista de inscrições do curso informado.</returns>
        /// <response code="200">Inscrições retornadas com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário sem permissão para acessar este recurso.</response>
        [HttpGet("curso/{cursoId:guid}")]
        [Authorize(Roles = "Tutor,Admin")]
        [ProducesResponseType(typeof(IEnumerable<InscricaoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObterPorCurso(Guid cursoId)
        {
            var inscricoes = await _inscricaoService.ObterPorCursoAsync(cursoId);
            return Ok(inscricoes);
        }
    }
}
