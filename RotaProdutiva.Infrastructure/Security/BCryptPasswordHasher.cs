using RotaProdutiva.Application.Interfaces;

namespace RotaProdutiva.Infrastructure.Security
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string HashSenha(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public bool VerificarSenha(string senha, string senhaHash)
        {
            return BCrypt.Net.BCrypt.Verify(senha, senhaHash);
        }
    }
}
