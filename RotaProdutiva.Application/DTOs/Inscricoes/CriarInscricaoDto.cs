namespace RotaProdutiva.Application.DTOs.Inscricoes
{
    public class CriarInscricaoDto
    {
        public Guid CursoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string WhatsApp { get; set; } = string.Empty;
    }
}
