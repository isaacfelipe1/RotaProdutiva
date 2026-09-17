namespace RotaProdutiva.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public Guid UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string StatusAprovacao { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
