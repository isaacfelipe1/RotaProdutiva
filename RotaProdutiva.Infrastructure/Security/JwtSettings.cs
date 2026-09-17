namespace RotaProdutiva.Infrastructure.Security
{
    public class JwtSettings
    {
        public string Chave { get; set; } = string.Empty;
        public string Emissor { get; set; } = string.Empty;
        public string Audiencia { get; set; } = string.Empty;
        public int ExpiracaoMinutos { get; set; } = 60;
    }
}
