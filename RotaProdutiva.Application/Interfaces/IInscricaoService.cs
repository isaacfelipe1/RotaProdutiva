using RotaProdutiva.Application.DTOs.Inscricoes;

namespace RotaProdutiva.Application.Interfaces
{
    public interface IInscricaoService
    {
        Task<InscricaoDto> CriarAsync(CriarInscricaoDto dto);
        Task<List<InscricaoDto>> ObterPorCursoAsync(Guid cursoId);
    }
}
