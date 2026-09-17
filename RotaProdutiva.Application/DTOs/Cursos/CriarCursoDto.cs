namespace RotaProdutiva.Application.DTOs.Cursos
{
    public class CriarCursoDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public int CargaHoraria { get; set; }
        public int Vagas { get; set; }
        public string Modalidade { get; set; } = string.Empty;
        public string? Local { get; set; }
    }
}
