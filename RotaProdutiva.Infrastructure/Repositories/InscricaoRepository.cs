using Microsoft.EntityFrameworkCore;
using RotaProdutiva.Domain.Entities;
using RotaProdutiva.Domain.Enums;
using RotaProdutiva.Domain.Interfaces;
using RotaProdutiva.Infrastructure.Data;

namespace RotaProdutiva.Infrastructure.Repositories
{
    public class InscricaoRepository : IInscricaoRepository
    {
        private readonly AppDbContext _context;

        public InscricaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Inscricao?> ObterPorIdAsync(Guid id)
        {
            return await _context.Inscricoes
                .Include(i => i.Curso)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<Inscricao>> ObterPorCursoAsync(Guid cursoId)
        {
            return await _context.Inscricoes
                .Include(i => i.Curso)
                .Where(i => i.CursoId == cursoId)
                .OrderByDescending(i => i.CriadoEm)
                .ToListAsync();
        }

        public async Task<bool> ExisteInscricaoAtivaAsync(string email, Guid cursoId)
        {
            return await _context.Inscricoes
                .AnyAsync(i => i.Email == email
                    && i.CursoId == cursoId
                    && i.Status == StatusInscricao.Ativa);
        }

        public async Task AdicionarAsync(Inscricao inscricao)
        {
            await _context.Inscricoes.AddAsync(inscricao);
            await _context.SaveChangesAsync();
        }
    }
}
