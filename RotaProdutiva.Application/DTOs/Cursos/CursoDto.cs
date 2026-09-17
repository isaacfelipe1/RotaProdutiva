namespace RotaProdutiva.Application.DTOs.Cursos
{
    public class CursoDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public Guid TutorId { get; set; }
        public string? TutorNome { get; set; }
        public DateTime DataInicio { get; set; }
        public int CargaHoraria { get; set; }
        public int Vagas { get; set; }
        public int VagasDisponiveis { get; set; }
        public string Modalidade { get; set; } = string.Empty;
        public string? Local { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
