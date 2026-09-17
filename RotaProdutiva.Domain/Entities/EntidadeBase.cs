namespace RotaProdutiva.Domain.Entities
{
    public abstract class EntidadeBase
    {
        public Guid Id { get; protected set; }
        public DateTime CriadoEm { get; protected set; }

        protected EntidadeBase()
        {
            Id = Guid.NewGuid();
            CriadoEm = DateTime.UtcNow;
        }
    }
}
