using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RotaProdutiva.Domain.Entities;
using RotaProdutiva.Domain.Enums;
using RotaProdutiva.Domain.Interfaces;
using RotaProdutiva.Infrastructure.Data;

namespace RotaProdutiva.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email.Trim().ToLower());
        }

        public async Task<Usuario?> ObterPorIdAsync(Guid id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AdicionarAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Email == email.Trim().ToLower());
        }

        public async Task<List<Usuario>> ObterTutoresPorStatusAsync(StatusAprovacaoTutor status)
        {
            return await _context.Usuarios
                .Where(u => u.Tipo == TipoUsuario.Tutor && u.StatusAprovacao == status)
                .ToListAsync();
        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
