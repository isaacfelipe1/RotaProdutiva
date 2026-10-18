namespace RotaProdutiva.Application.DTOs.Admin
{
    public class TutorDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string WhatsApp { get; set; } = string.Empty;
        public string StatusAprovacao { get; set; } = string.Empty;
        public int TotalCursos { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
