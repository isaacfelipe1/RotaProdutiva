using RotaProdutiva.Domain.Enums;

namespace RotaProdutiva.Domain.Entities
{
    public class Inscricao : EntidadeBase
    {
        public string Nome { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string WhatsApp { get; private set; } = string.Empty;

        public Guid CursoId { get; private set; }
        public Curso? Curso { get; private set; }

        public StatusInscricao Status { get; private set; }

        protected Inscricao() { }

        public Inscricao(string nome, string email, string whatsApp, Guid cursoId)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório.", nameof(nome));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email é obrigatório.", nameof(email));

            if (string.IsNullOrWhiteSpace(whatsApp))
                throw new ArgumentException("WhatsApp é obrigatório.", nameof(whatsApp));

            if (cursoId == Guid.Empty)
                throw new ArgumentException("CursoId é obrigatório.", nameof(cursoId));

            Nome = nome;
            Email = email.Trim().ToLowerInvariant();
            WhatsApp = whatsApp;
            CursoId = cursoId;
            Status = StatusInscricao.Ativa;
        }

        public void Cancelar()
        {
            Status = StatusInscricao.Cancelada;
        }

        public void Concluir()
        {
            Status = StatusInscricao.Concluida;
        }
    }
}
