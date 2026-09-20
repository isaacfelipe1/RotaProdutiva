using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RotaProdutiva.Application.DTOs.Admin;
using RotaProdutiva.Application.Interfaces;
using RotaProdutiva.Domain.Exceptions;

namespace RotaProdutiva.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Obtém a lista de solicitações de tutores pendentes de aprovação.
        /// </summary>
        /// <returns>Lista de solicitações pendentes.</returns>
        /// <response code="200">Solicitações retornadas com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário sem permissão de administrador.</response>
        [HttpGet("tutores/pendentes")]
        [ProducesResponseType(typeof(IEnumerable<SolicitacaoTutorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObterTutoresPendentes()
        {
            var tutores = await _adminService.ObterTutoresPendentesAsync();
            return Ok(tutores);
        }

        /// <summary>
        /// Aprova a solicitação de um tutor.
        /// </summary>
        /// <param name="id">Identificador da solicitação/tutor.</param>
        /// <response code="204">Tutor aprovado com sucesso.</response>
        /// <response code="400">Solicitação inválida ou já processada.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário sem permissão de administrador.</response>
        [HttpPost("tutores/{id:guid}/aprovar")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AprovarTutor(Guid id)
        {
            try
            {
                await _adminService.AprovarTutorAsync(id);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Rejeita a solicitação de um tutor.
        /// </summary>
        /// <param name="id">Identificador da solicitação/tutor.</param>
        /// <response code="204">Tutor rejeitado com sucesso.</response>
        /// <response code="400">Solicitação inválida ou já processada.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário sem permissão de administrador.</response>
        [HttpPost("tutores/{id:guid}/rejeitar")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RejeitarTutor(Guid id)
        {
            try
            {
                await _adminService.RejeitarTutorAsync(id);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}
