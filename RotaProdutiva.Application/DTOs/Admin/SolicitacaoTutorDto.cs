namespace RotaProdutiva.Application.DTOs.Admin
{
    public class SolicitacaoTutorDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string WhatsApp { get; set; } = string.Empty;
        public string StatusAprovacao { get; set; } = string.Empty;
        public DateTime CriadoEm { get; set; }
    }
}
