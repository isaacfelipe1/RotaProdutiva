using Moq;
using RotaProdutiva.Application.DTOs.Cursos;
using RotaProdutiva.Application.Services;
using RotaProdutiva.Domain.Entities;
using RotaProdutiva.Domain.Enums;
using RotaProdutiva.Domain.Exceptions;
using RotaProdutiva.Domain.Interfaces;

namespace RotaProdutiva.Tests.Application
{
    public class CursoServiceTests
    {
        private readonly Mock<ICursoRepository> _cursoRepositoryMock = new();
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock = new();
        private readonly CursoService _cursoService;

        public CursoServiceTests()
        {
            _cursoService = new CursoService(_cursoRepositoryMock.Object, _usuarioRepositoryMock.Object);
        }

        [Fact]
        public async Task Deve_Criar_Curso_Quando_Usuario_E_Tutor()
        {
            var tutorId = Guid.NewGuid();
            var tutor = new Usuario("Maria", "maria@example.com", "hash", "11988888888", TipoUsuario.Tutor);
            tutor.AprovarTutor();
            var dto = new CriarCursoDto
            {
                Titulo = "C# Avançado",
                Descricao = "Aprenda C#",
                DataInicio = DateTime.UtcNow.AddDays(10),
                CargaHoraria = 20,
                Vagas = 15,
                Modalidade = "Online"
            };

            _usuarioRepositoryMock.Setup(r => r.ObterPorIdAsync(tutorId)).ReturnsAsync(tutor);

            var resultado = await _cursoService.CriarAsync(dto, tutorId);

            Assert.Equal("C# Avançado", resultado.Titulo);
            _cursoRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Curso>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Usuario_Nao_E_Tutor()
        {
            var jovemId = Guid.NewGuid();
            var jovem = new Usuario("João", "joao@example.com", "hash", "11999999999", TipoUsuario.Jovem);
            var dto = new CriarCursoDto
            {
                Titulo = "C# Avançado",
                Descricao = "Aprenda C#",
                DataInicio = DateTime.UtcNow.AddDays(10),
                CargaHoraria = 20,
                Vagas = 15,
                Modalidade = "Online"
            };

            _usuarioRepositoryMock.Setup(r => r.ObterPorIdAsync(jovemId)).ReturnsAsync(jovem);

            await Assert.ThrowsAsync<DomainException>(() => _cursoService.CriarAsync(dto, jovemId));
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Usuario_Nao_Encontrado()
        {
            var tutorId = Guid.NewGuid();
            var dto = new CriarCursoDto
            {
                Titulo = "C# Avançado",
                Descricao = "Aprenda C#",
                DataInicio = DateTime.UtcNow.AddDays(10),
                CargaHoraria = 20,
                Vagas = 15,
                Modalidade = "Online"
            };

            _usuarioRepositoryMock.Setup(r => r.ObterPorIdAsync(tutorId)).ReturnsAsync((Usuario?)null);

            await Assert.ThrowsAsync<DomainException>(() => _cursoService.CriarAsync(dto, tutorId));
        }

        [Fact]
        public async Task Deve_Obter_Curso_Por_Id()
        {
            var tutorId = Guid.NewGuid();
            var curso = new Curso("C# Avançado", "Aprenda C#", tutorId, DateTime.UtcNow.AddDays(10), 20, 15, ModalidadeCurso.Online, null);

            _cursoRepositoryMock.Setup(r => r.ObterPorIdAsync(curso.Id)).ReturnsAsync(curso);

            var resultado = await _cursoService.ObterPorIdAsync(curso.Id);

            Assert.Equal(curso.Id, resultado.Id);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Curso_Nao_Encontrado()
        {
            var cursoId = Guid.NewGuid();

            _cursoRepositoryMock.Setup(r => r.ObterPorIdAsync(cursoId)).ReturnsAsync((Curso?)null);

            await Assert.ThrowsAsync<DomainException>(() => _cursoService.ObterPorIdAsync(cursoId));
        }
    }
}
