using RotaProdutiva.Domain.Entities;
using RotaProdutiva.Domain.Entities;
using RotaProdutiva.Domain.Enums;

namespace RotaProdutiva.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task<Usuario?> ObterPorIdAsync(Guid id);
        Task AdicionarAsync(Usuario usuario);
        Task<bool> ExisteEmailAsync(string email);
        Task<List<Usuario>> ObterTutoresPorStatusAsync(StatusAprovacaoTutor status);
        Task AtualizarAsync(Usuario usuario);
    }
}
