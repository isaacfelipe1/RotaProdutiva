using RotaProdutiva.Application.DTOs.Auth;

namespace RotaProdutiva.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegistrarAsync(RegistrarUsuarioDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}
