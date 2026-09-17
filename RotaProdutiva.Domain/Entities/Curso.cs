using RotaProdutiva.Domain.Enums;

namespace RotaProdutiva.Domain.Entities
{
    public class Curso : EntidadeBase
    {
        public string Titulo { get; private set; } = string.Empty;
        public string Descricao { get; private set; } = string.Empty;
        public Guid TutorId { get; private set; }
        public Usuario? Tutor { get; private set; }

        public DateTime DataInicio { get; private set; }
        public int CargaHoraria { get; private set; }
        public int Vagas { get; private set; }
        public ModalidadeCurso Modalidade { get; private set; }
        public string? Local { get; private set; }

        public ICollection<Inscricao> Inscricoes { get; private set; } = new List<Inscricao>();

        public int VagasDisponiveis => Vagas - Inscricoes.Count(i => i.Status != StatusInscricao.Cancelada);

        protected Curso() { }

        public Curso(
            string titulo,
            string descricao,
            Guid tutorId,
            DateTime dataInicio,
            int cargaHoraria,
            int vagas,
            ModalidadeCurso modalidade,
            string? local)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("Título é obrigatório.", nameof(titulo));

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição é obrigatória.", nameof(descricao));

            if (tutorId == Guid.Empty)
                throw new ArgumentException("TutorId é obrigatório.", nameof(tutorId));

            if (dataInicio <= DateTime.UtcNow)
                throw new ArgumentException("Data de início deve ser futura.", nameof(dataInicio));

            if (cargaHoraria <= 0)
                throw new ArgumentException("Carga horária deve ser maior que zero.", nameof(cargaHoraria));

            if (vagas <= 0)
                throw new ArgumentException("Vagas deve ser maior que zero.", nameof(vagas));

            if (modalidade == ModalidadeCurso.Presencial && string.IsNullOrWhiteSpace(local))
                throw new ArgumentException("Local é obrigatório para cursos presenciais.", nameof(local));

            Titulo = titulo;
            Descricao = descricao;
            TutorId = tutorId;
            DataInicio = dataInicio;
            CargaHoraria = cargaHoraria;
            Vagas = vagas;
            Modalidade = modalidade;
            Local = modalidade == ModalidadeCurso.Online ? null : local;
        }
    }
}

