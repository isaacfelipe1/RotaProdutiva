using RotaProdutiva.Application.DTOs.Auth;
using RotaProdutiva.Application.Interfaces;
using RotaProdutiva.Domain.Entities;
using RotaProdutiva.Domain.Enums;
using RotaProdutiva.Domain.Exceptions;
using RotaProdutiva.Domain.Interfaces;

namespace RotaProdutiva.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public AuthService(
            IUsuarioRepository usuarioRepository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> RegistrarAsync(RegistrarUsuarioDto dto)
        {
            if (await _usuarioRepository.ExisteEmailAsync(dto.Email))
                throw new DomainException("Já existe um usuário cadastrado com este e-mail.");

            var senhaHash = _passwordHasher.HashSenha(dto.Senha);

            var usuario = new Usuario(dto.Nome, dto.Email, senhaHash, dto.WhatsApp, dto.Tipo);

            await _usuarioRepository.AdicionarAsync(usuario);

            var token = _tokenService.GerarToken(usuario);

            return MontarResposta(usuario, token);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(dto.Email.Trim().ToLowerInvariant());

            if (usuario is null || !_passwordHasher.VerificarSenha(dto.Senha, usuario.SenhaHash))
                throw new DomainException("E-mail ou senha inválidos.");

            if (usuario.EhTutor() && usuario.StatusAprovacao == StatusAprovacaoTutor.Rejeitado)
                throw new DomainException("Sua solicitação de tutor foi rejeitada.");

            var token = _tokenService.GerarToken(usuario);

            return MontarResposta(usuario, token);
        }

        private static AuthResponseDto MontarResposta(Usuario usuario, string token)
        {
            return new AuthResponseDto
            {
                UsuarioId = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Tipo = usuario.Tipo.ToString(),
                StatusAprovacao = usuario.StatusAprovacao.ToString(),
                Token = token
            };
        }
    }
}
