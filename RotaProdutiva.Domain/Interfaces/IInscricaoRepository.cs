using RotaProdutiva.Domain.Entities;

namespace RotaProdutiva.Domain.Interfaces
{
    public interface IInscricaoRepository
    {
        Task<Inscricao?> ObterPorIdAsync(Guid id);
        Task<List<Inscricao>> ObterPorCursoAsync(Guid cursoId);
        Task<bool> ExisteInscricaoAtivaAsync(string email, Guid cursoId);
        Task AdicionarAsync(Inscricao inscricao);
    }
}
