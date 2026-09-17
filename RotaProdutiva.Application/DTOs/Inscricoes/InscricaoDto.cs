namespace RotaProdutiva.Application.DTOs.Inscricoes
{
    public class InscricaoDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string WhatsApp { get; set; } = string.Empty;
        public Guid CursoId { get; set; }
        public string? CursoTitulo { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CriadoEm { get; set; }
    }
}
