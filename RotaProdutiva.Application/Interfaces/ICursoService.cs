using RotaProdutiva.Application.DTOs.Cursos;

namespace RotaProdutiva.Application.Interfaces
{
    public interface ICursoService
    {
        Task<CursoDto> CriarAsync(CriarCursoDto dto, Guid tutorId);
        Task<List<CursoDto>> ObterTodosAsync();
        Task<CursoDto> ObterPorIdAsync(Guid id);
        Task<List<CursoDto>> ObterPorTutorAsync(Guid tutorId);
    }
}
