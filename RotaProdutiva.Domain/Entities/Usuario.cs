using RotaProdutiva.Domain.Enums;
using RotaProdutiva.Domain.Enums;

namespace RotaProdutiva.Domain.Entities
{
    public class Usuario : EntidadeBase
    {
        public string Nome { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string SenhaHash { get; private set; } = string.Empty;
        public string WhatsApp { get; private set; } = string.Empty;
        public TipoUsuario Tipo { get; private set; }
        public StatusAprovacaoTutor StatusAprovacao { get; private set; }

        public ICollection<Curso> Cursos { get; private set; } = new List<Curso>();

        protected Usuario() { }

        public Usuario(string nome, string email, string senhaHash, string whatsApp, TipoUsuario tipo)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório.", nameof(nome));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email é obrigatório.", nameof(email));

            if (string.IsNullOrWhiteSpace(senhaHash))
                throw new ArgumentException("Senha é obrigatória.", nameof(senhaHash));

            Nome = nome;
            Email = email.Trim().ToLowerInvariant();
            SenhaHash = senhaHash;
            WhatsApp = whatsApp;
            Tipo = tipo;
            StatusAprovacao = tipo == TipoUsuario.Tutor
                ? StatusAprovacaoTutor.Pendente
                : StatusAprovacaoTutor.NaoAplicavel;
        }

        public bool EhTutor() => Tipo == TipoUsuario.Tutor;
        public bool EhAluno() => Tipo == TipoUsuario.Aluno;
        public bool EhAdmin() => Tipo == TipoUsuario.Admin;
        public bool EhTutorAprovado() => Tipo == TipoUsuario.Tutor && StatusAprovacao == StatusAprovacaoTutor.Aprovado;

        public void AprovarTutor()
        {
            if (!EhTutor())
                throw new Exceptions.DomainException("Usuário não é um candidato a tutor.");

            StatusAprovacao = StatusAprovacaoTutor.Aprovado;
        }

        public void RejeitarTutor()
        {
            if (!EhTutor())
                throw new Exceptions.DomainException("Usuário não é um candidato a tutor.");

            StatusAprovacao = StatusAprovacaoTutor.Rejeitado;
        }
    }
}
