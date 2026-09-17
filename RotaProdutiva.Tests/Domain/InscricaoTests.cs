using RotaProdutiva.Domain.Entities;
using RotaProdutiva.Domain.Enums;

namespace RotaProdutiva.Tests.Domain
{
    public class InscricaoTests
    {
        [Fact]
        public void Deve_Criar_Inscricao_Ativa_Por_Padrao()
        {
            var cursoId = Guid.NewGuid();

            var inscricao = new Inscricao("João", "joao@example.com", "11999999999", cursoId);

            Assert.Equal("João", inscricao.Nome);
            Assert.Equal("joao@example.com", inscricao.Email);
            Assert.Equal("11999999999", inscricao.WhatsApp);
            Assert.Equal(cursoId, inscricao.CursoId);
            Assert.Equal(StatusInscricao.Ativa, inscricao.Status);
        }

        [Fact]
        public void Deve_Cancelar_Inscricao()
        {
            var inscricao = new Inscricao("João", "joao@example.com", "11999999999", Guid.NewGuid());

            inscricao.Cancelar();

            Assert.Equal(StatusInscricao.Cancelada, inscricao.Status);
        }

        [Fact]
        public void Deve_Concluir_Inscricao()
        {
            var inscricao = new Inscricao("João", "joao@example.com", "11999999999", Guid.NewGuid());

            inscricao.Concluir();

            Assert.Equal(StatusInscricao.Concluida, inscricao.Status);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Nome_Vazio()
        {
            Assert.Throws<ArgumentException>(() =>
                new Inscricao("", "joao@example.com", "11999999999", Guid.NewGuid()));
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Email_Vazio()
        {
            Assert.Throws<ArgumentException>(() =>
                new Inscricao("João", "", "11999999999", Guid.NewGuid()));
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_WhatsApp_Vazio()
        {
            Assert.Throws<ArgumentException>(() =>
                new Inscricao("João", "joao@example.com", "", Guid.NewGuid()));
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_CursoId_Vazio()
        {
            Assert.Throws<ArgumentException>(() =>
                new Inscricao("João", "joao@example.com", "11999999999", Guid.Empty));
        }
    }
}
