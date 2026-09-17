using Moq;
using RotaProdutiva.Application.DTOs.Inscricoes;
using RotaProdutiva.Application.Services;
using RotaProdutiva.Domain.Entities;
using RotaProdutiva.Domain.Enums;
using RotaProdutiva.Domain.Exceptions;
using RotaProdutiva.Domain.Interfaces;
using Xunit;

namespace RotaProdutiva.Tests.Application
{
    public class InscricaoServiceTests
    {
        private readonly Mock<IInscricaoRepository> _inscricaoRepositoryMock = new();
        private readonly Mock<ICursoRepository> _cursoRepositoryMock = new();
        private readonly InscricaoService _inscricaoService;

        public InscricaoServiceTests()
        {
            _inscricaoService = new InscricaoService(
                _inscricaoRepositoryMock.Object,
                _cursoRepositoryMock.Object);
        }

        [Fact]
        public async Task Deve_Criar_Inscricao_Com_Dados_De_Contato_Validos()
        {
            var curso = new Curso("C# Avançado", "Aprenda C#", Guid.NewGuid(), DateTime.UtcNow.AddDays(10), 20, 15, ModalidadeCurso.Online, null);
            var dto = new CriarInscricaoDto
            {
                CursoId = curso.Id,
                Nome = "João",
                Email = "joao@example.com",
                WhatsApp = "11999999999"
            };

            _cursoRepositoryMock.Setup(r => r.ObterPorIdAsync(curso.Id)).ReturnsAsync(curso);
            _inscricaoRepositoryMock.Setup(r => r.ExisteInscricaoAtivaAsync(dto.Email, curso.Id)).ReturnsAsync(false);

            var resultado = await _inscricaoService.CriarAsync(dto);

            Assert.Equal(dto.Nome, resultado.Nome);
            Assert.Equal(dto.Email, resultado.Email);
            Assert.Equal(curso.Id, resultado.CursoId);
            _inscricaoRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Inscricao>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Curso_Nao_Encontrado()
        {
            var dto = new CriarInscricaoDto
            {
                CursoId = Guid.NewGuid(),
                Nome = "João",
                Email = "joao@example.com",
                WhatsApp = "11999999999"
            };

            _cursoRepositoryMock.Setup(r => r.ObterPorIdAsync(dto.CursoId)).ReturnsAsync((Curso?)null);

            await Assert.ThrowsAsync<DomainException>(() => _inscricaoService.CriarAsync(dto));
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Ja_Inscrito()
        {
            var curso = new Curso("C# Avançado", "Aprenda C#", Guid.NewGuid(), DateTime.UtcNow.AddDays(10), 20, 15, ModalidadeCurso.Online, null);
            var dto = new CriarInscricaoDto
            {
                CursoId = curso.Id,
                Nome = "João",
                Email = "joao@example.com",
                WhatsApp = "11999999999"
            };

            _cursoRepositoryMock.Setup(r => r.ObterPorIdAsync(curso.Id)).ReturnsAsync(curso);
            _inscricaoRepositoryMock.Setup(r => r.ExisteInscricaoAtivaAsync(dto.Email, curso.Id)).ReturnsAsync(true);

            await Assert.ThrowsAsync<DomainException>(() => _inscricaoService.CriarAsync(dto));
        }
    }
}
