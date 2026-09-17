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
