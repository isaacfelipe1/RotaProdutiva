using RotaProdutiva.Application.DTOs.Cursos;
using RotaProdutiva.Application.Interfaces;
using RotaProdutiva.Domain.Entities;
using RotaProdutiva.Domain.Enums;
using RotaProdutiva.Domain.Exceptions;
using RotaProdutiva.Domain.Interfaces;

namespace RotaProdutiva.Application.Services
{
    public class CursoService : ICursoService
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public CursoService(ICursoRepository cursoRepository, IUsuarioRepository usuarioRepository)
        {
            _cursoRepository = cursoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<CursoDto> CriarAsync(CriarCursoDto dto, Guid tutorId)
        {
            var tutor = await _usuarioRepository.ObterPorIdAsync(tutorId);

            if (tutor is null || !(tutor.EhTutor() || tutor.EhAdmin()))
                throw new DomainException("Apenas tutores podem criar cursos.");

            if (tutor.EhTutor() && !tutor.EhTutorAprovado())
                throw new DomainException("Seu cadastro de tutor ainda não foi aprovado.");

            if (!Enum.TryParse<ModalidadeCurso>(dto.Modalidade, true, out var modalidade))
                throw new DomainException("Modalidade inválida. Utilize 'Presencial' ou 'Online'.");

            var curso = new Curso(
                dto.Titulo,
                dto.Descricao,
                tutorId,
                dto.DataInicio,
                dto.CargaHoraria,
                dto.Vagas,
                modalidade,
                dto.Local);

            await _cursoRepository.AdicionarAsync(curso);

            return MontarDto(curso, tutor.Nome);
        }

        public async Task<List<CursoDto>> ObterTodosAsync()
        {
            var cursos = await _cursoRepository.ObterTodosAsync();
            return cursos.Select(c => MontarDto(c, c.Tutor?.Nome)).ToList();
        }

        public async Task<CursoDto> ObterPorIdAsync(Guid id)
        {
            var curso = await _cursoRepository.ObterPorIdAsync(id)
                ?? throw new DomainException("Curso não encontrado.");

            return MontarDto(curso, curso.Tutor?.Nome);
        }

        public async Task<List<CursoDto>> ObterPorTutorAsync(Guid tutorId)
        {
            var cursos = await _cursoRepository.ObterPorTutorAsync(tutorId);
            return cursos.Select(c => MontarDto(c, c.Tutor?.Nome)).ToList();
        }

        private static CursoDto MontarDto(Curso curso, string? tutorNome)
        {
            return new CursoDto
            {
                Id = curso.Id,
                Titulo = curso.Titulo,
                Descricao = curso.Descricao,
                TutorId = curso.TutorId,
                TutorNome = tutorNome,
                DataInicio = curso.DataInicio,
                CargaHoraria = curso.CargaHoraria,
                Vagas = curso.Vagas,
                VagasDisponiveis = curso.VagasDisponiveis,
                Modalidade = curso.Modalidade.ToString(),
                Local = curso.Local,
                CriadoEm = curso.CriadoEm
            };
        }
    }
}
