using RotaProdutiva.Application.DTOs.Admin;

namespace RotaProdutiva.Application.Interfaces
{
    public interface IAdminService
    {
        Task<List<SolicitacaoTutorDto>> ObterTutoresPendentesAsync();
        Task AprovarTutorAsync(Guid usuarioId);
        Task RejeitarTutorAsync(Guid usuarioId);
    }
}
