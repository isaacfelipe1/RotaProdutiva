using RotaProdutiva.Application.DTOs.Admin;
using RotaProdutiva.Application.Interfaces;
using RotaProdutiva.Domain.Enums;
using RotaProdutiva.Domain.Exceptions;
using RotaProdutiva.Domain.Interfaces;

namespace RotaProdutiva.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AdminService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<List<SolicitacaoTutorDto>> ObterTutoresPendentesAsync()
        {
            var tutores = await _usuarioRepository.ObterTutoresPorStatusAsync(StatusAprovacaoTutor.Pendente);

            return tutores.Select(t => new SolicitacaoTutorDto
            {
                Id = t.Id,
                Nome = t.Nome,
                Email = t.Email,
                WhatsApp = t.WhatsApp,
                StatusAprovacao = t.StatusAprovacao.ToString(),
                CriadoEm = t.CriadoEm
            }).ToList();
        }

        public async Task AprovarTutorAsync(Guid usuarioId)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId)
                ?? throw new DomainException("Usuário não encontrado.");

            usuario.AprovarTutor();

            await _usuarioRepository.AtualizarAsync(usuario);
        }

        public async Task RejeitarTutorAsync(Guid usuarioId)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId)
                ?? throw new DomainException("Usuário não encontrado.");

            usuario.RejeitarTutor();

            await _usuarioRepository.AtualizarAsync(usuario);
        }
    }
}
