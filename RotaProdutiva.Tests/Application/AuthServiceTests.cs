using Moq;
using RotaProdutiva.Application.DTOs.Auth;
using RotaProdutiva.Application.Interfaces;
using RotaProdutiva.Application.Services;
using RotaProdutiva.Domain.Entities;
using RotaProdutiva.Domain.Enums;
using RotaProdutiva.Domain.Exceptions;
using RotaProdutiva.Domain.Interfaces;

namespace RotaProdutiva.Tests.Application
{
    public class AuthServiceTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _authService = new AuthService(
                _usuarioRepositoryMock.Object,
                _passwordHasherMock.Object,
                _tokenServiceMock.Object);
        }

        [Fact]
        public async Task Deve_Registrar_Usuario_Com_Sucesso()
        {
            var dto = new RegistrarUsuarioDto
            {
                Nome = "João Silva",
                Email = "joao@example.com",
                Senha = "senha123",
                WhatsApp = "11999999999",
                Tipo = TipoUsuario.Jovem
            };

            _usuarioRepositoryMock.Setup(r => r.ExisteEmailAsync(dto.Email)).ReturnsAsync(false);
            _passwordHasherMock.Setup(p => p.HashSenha(dto.Senha)).Returns("hash_senha");
            _tokenServiceMock.Setup(t => t.GerarToken(It.IsAny<Usuario>())).Returns("token_gerado");

            var resultado = await _authService.RegistrarAsync(dto);

            Assert.Equal("joao@example.com", resultado.Email);
            Assert.Equal("token_gerado", resultado.Token);
            _usuarioRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Email_Ja_Cadastrado()
        {
            var dto = new RegistrarUsuarioDto
            {
                Nome = "João Silva",
                Email = "joao@example.com",
                Senha = "senha123",
                WhatsApp = "11999999999",
                Tipo = TipoUsuario.Jovem
            };

            _usuarioRepositoryMock.Setup(r => r.ExisteEmailAsync(dto.Email)).ReturnsAsync(true);

            await Assert.ThrowsAsync<DomainException>(() => _authService.RegistrarAsync(dto));
        }

        [Fact]
        public async Task Deve_Fazer_Login_Com_Sucesso()
        {
            var usuario = new Usuario("João Silva", "joao@example.com", "hash_senha", "11999999999", TipoUsuario.Jovem);
            var dto = new LoginDto { Email = "joao@example.com", Senha = "senha123" };

            _usuarioRepositoryMock.Setup(r => r.ObterPorEmailAsync(dto.Email)).ReturnsAsync(usuario);
            _passwordHasherMock.Setup(p => p.VerificarSenha(dto.Senha, usuario.SenhaHash)).Returns(true);
            _tokenServiceMock.Setup(t => t.GerarToken(usuario)).Returns("token_gerado");

            var resultado = await _authService.LoginAsync(dto);

            Assert.Equal("token_gerado", resultado.Token);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Usuario_Nao_Encontrado_No_Login()
        {
            var dto = new LoginDto { Email = "naoexiste@example.com", Senha = "senha123" };

            _usuarioRepositoryMock.Setup(r => r.ObterPorEmailAsync(dto.Email)).ReturnsAsync((Usuario?)null);

            await Assert.ThrowsAsync<DomainException>(() => _authService.LoginAsync(dto));
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Senha_Incorreta()
        {
            var usuario = new Usuario("João Silva", "joao@example.com", "hash_senha", "11999999999", TipoUsuario.Jovem);
            var dto = new LoginDto { Email = "joao@example.com", Senha = "senhaerrada" };

            _usuarioRepositoryMock.Setup(r => r.ObterPorEmailAsync(dto.Email)).ReturnsAsync(usuario);
            _passwordHasherMock.Setup(p => p.VerificarSenha(dto.Senha, usuario.SenhaHash)).Returns(false);

            await Assert.ThrowsAsync<DomainException>(() => _authService.LoginAsync(dto));
        }
    }
}
