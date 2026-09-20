using RotaProdutiva.Domain.Entities;
using RotaProdutiva.Domain.Enums;

namespace RotaProdutiva.Tests.Domain
{
    public class UsuarioTests
    {
        [Fact]
        public void Deve_Criar_Usuario_Com_Dados_Validos()
        {
            var usuario = new Usuario("João Silva", "Joao@Example.com", "hash123", "11999999999", TipoUsuario.Aluno);

            Assert.Equal("João Silva", usuario.Nome);
            Assert.Equal("joao@example.com", usuario.Email);
            Assert.True(usuario.EhAluno());
            Assert.False(usuario.EhTutor());
            Assert.False(usuario.EhAdmin());
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Nome_Vazio()
        {
            Assert.Throws<ArgumentException>(() =>
                new Usuario("", "joao@example.com", "hash123", "11999999999", TipoUsuario.Aluno));
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Email_Vazio()
        {
            Assert.Throws<ArgumentException>(() =>
                new Usuario("João Silva", "", "hash123", "11999999999", TipoUsuario.Aluno));
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Senha_Vazia()
        {
            Assert.Throws<ArgumentException>(() =>
                new Usuario("João Silva", "joao@example.com", "", "11999999999", TipoUsuario.Aluno));
        }

        [Theory]
        [InlineData(TipoUsuario.Tutor)]
        public void Deve_Identificar_Tutor_Corretamente(TipoUsuario tipo)
        {
            var usuario = new Usuario("Maria", "maria@example.com", "hash123", "11988888888", tipo);

            Assert.True(usuario.EhTutor());
        }

        [Fact]
        public void Deve_Identificar_Admin_Corretamente()
        {
            var usuario = new Usuario("Admin", "admin@example.com", "hash123", "", TipoUsuario.Admin);

            Assert.True(usuario.EhAdmin());
        }
    }
}
