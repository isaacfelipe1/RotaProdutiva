using RotaProdutiva.Domain.Entities;
using RotaProdutiva.Domain.Entities;
using RotaProdutiva.Domain.Enums;

namespace RotaProdutiva.Tests.Domain
{
    public class CursoTests
    {
        [Fact]
        public void Deve_Criar_Curso_Com_Dados_Validos()
        {
            var tutorId = Guid.NewGuid();
            var curso = new Curso("C# Avançado", "Aprenda C# do zero ao avançado", tutorId, DateTime.UtcNow.AddDays(10), 20, 15, ModalidadeCurso.Online, null);

            Assert.Equal("C# Avançado", curso.Titulo);
            Assert.Equal("Aprenda C# do zero ao avançado", curso.Descricao);
            Assert.Equal(tutorId, curso.TutorId);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Titulo_Vazio()
        {
            Assert.Throws<ArgumentException>(() =>
                new Curso("", "Descrição válida", Guid.NewGuid(), DateTime.UtcNow.AddDays(10), 20, 15, ModalidadeCurso.Online, null));
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Descricao_Vazia()
        {
            Assert.Throws<ArgumentException>(() =>
                new Curso("Título válido", "", Guid.NewGuid(), DateTime.UtcNow.AddDays(10), 20, 15, ModalidadeCurso.Online, null));
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_TutorId_Vazio()
        {
            Assert.Throws<ArgumentException>(() =>
                new Curso("Título válido", "Descrição válida", Guid.Empty, DateTime.UtcNow.AddDays(10), 20, 15, ModalidadeCurso.Online, null));
        }
    }
}
