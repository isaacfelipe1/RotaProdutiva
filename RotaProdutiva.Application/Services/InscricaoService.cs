using RotaProdutiva.Application.DTOs.Inscricoes;
using RotaProdutiva.Application.Interfaces;
using RotaProdutiva.Domain.Entities;
using RotaProdutiva.Domain.Exceptions;
using RotaProdutiva.Domain.Interfaces;

namespace RotaProdutiva.Application.Services
{
    public class InscricaoService : IInscricaoService
    {
        private readonly IInscricaoRepository _inscricaoRepository;
        private readonly ICursoRepository _cursoRepository;

        public InscricaoService(
            IInscricaoRepository inscricaoRepository,
            ICursoRepository cursoRepository)
        {
            _inscricaoRepository = inscricaoRepository;
            _cursoRepository = cursoRepository;
        }

        public async Task<InscricaoDto> CriarAsync(CriarInscricaoDto dto)
        {
            var curso = await _cursoRepository.ObterPorIdAsync(dto.CursoId)
                ?? throw new DomainException("Curso não encontrado.");

            if (await _inscricaoRepository.ExisteInscricaoAtivaAsync(dto.Email, dto.CursoId))
                throw new DomainException("Este email já está inscrito neste curso.");

            if (curso.VagasDisponiveis <= 0)
                throw new DomainException("Não há vagas disponíveis para este curso.");

            var inscricao = new Inscricao(dto.Nome, dto.Email, dto.WhatsApp, dto.CursoId);

            await _inscricaoRepository.AdicionarAsync(inscricao);

            return MontarDto(inscricao, curso.Titulo);
        }

        public async Task<List<InscricaoDto>> ObterPorCursoAsync(Guid cursoId)
        {
            var inscricoes = await _inscricaoRepository.ObterPorCursoAsync(cursoId);
            return inscricoes.Select(i => MontarDto(i, i.Curso?.Titulo)).ToList();
        }

        private static InscricaoDto MontarDto(Inscricao inscricao, string? cursoTitulo)
        {
            return new InscricaoDto
            {
                Id = inscricao.Id,
                Nome = inscricao.Nome,
                Email = inscricao.Email,
                WhatsApp = inscricao.WhatsApp,
                CursoId = inscricao.CursoId,
                CursoTitulo = cursoTitulo,
                Status = inscricao.Status.ToString(),
                CriadoEm = inscricao.CriadoEm
            };
        }
    }
}
