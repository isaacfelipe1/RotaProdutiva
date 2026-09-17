namespace RotaProdutiva.Application.Interfaces
{
    public interface IPasswordHasher
    {
        string HashSenha(string senha);
        bool VerificarSenha(string senha, string senhaHash);
    }
}
