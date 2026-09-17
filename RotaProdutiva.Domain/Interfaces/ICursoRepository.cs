using RotaProdutiva.Domain.Entities;

namespace RotaProdutiva.Domain.Interfaces
{
    public interface ICursoRepository
    {
        Task<Curso?> ObterPorIdAsync(Guid id);
        Task<List<Curso>> ObterTodosAsync();
        Task<List<Curso>> ObterPorTutorAsync(Guid tutorId);
        Task AdicionarAsync(Curso curso);
    }
}
