using Microsoft.EntityFrameworkCore;
using RotaProdutiva.Domain.Entities;
using RotaProdutiva.Domain.Interfaces;
using RotaProdutiva.Infrastructure.Data;

namespace RotaProdutiva.Infrastructure.Repositories
{
    public class CursoRepository : ICursoRepository
    {
        private readonly AppDbContext _context;

        public CursoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Curso?> ObterPorIdAsync(Guid id)
        {
            return await _context.Cursos
                .Include(c => c.Tutor)
                .Include(c => c.Inscricoes)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Curso>> ObterTodosAsync()
        {
            return await _context.Cursos
                .Include(c => c.Tutor)
                .Include(c => c.Inscricoes)
                .OrderByDescending(c => c.CriadoEm)
                .ToListAsync();
        }

        public async Task<List<Curso>> ObterPorTutorAsync(Guid tutorId)
        {
            return await _context.Cursos
                .Include(c => c.Tutor)
                .Include(c => c.Inscricoes)
                .Where(c => c.TutorId == tutorId)
                .OrderByDescending(c => c.CriadoEm)
                .ToListAsync();
        }

        public async Task AdicionarAsync(Curso curso)
        {
            await _context.Cursos.AddAsync(curso);
            await _context.SaveChangesAsync();
        }
    }
}
