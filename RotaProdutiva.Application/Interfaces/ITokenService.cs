using RotaProdutiva.Domain.Entities;

namespace RotaProdutiva.Application.Interfaces
{
    public interface ITokenService
    {
        string GerarToken(Usuario usuario);
    }
}
