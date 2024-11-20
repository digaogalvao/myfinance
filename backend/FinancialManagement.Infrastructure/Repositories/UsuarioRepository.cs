using FinancialManagement.Domain.Entities;
using FinancialManagement.Domain.Interfaces;
using FinancialManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FinancialManagement.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _dbContext;

        public UsuarioRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Usuario>> GetUsuarios()
        {
            return await _dbContext.Usuarios.ToListAsync();
        }

        public async Task<Usuario> GetUsuario(int id)
        {
            var usuario = await _dbContext.Usuarios.SingleOrDefaultAsync(c => c.Id == id);

            return usuario;
        }

        public async Task<Usuario> LoginUsuario(string email, string senha)
        {
            var usuario = await _dbContext.Usuarios
                .SingleOrDefaultAsync(u => u.Email == email);

            if (usuario != null && BCrypt.Net.BCrypt.Verify(senha, usuario.Senha))
            {
                return usuario;
            }
            else
            {
                return null;
            }
        }

        public async Task<int> CreateUsuario(Usuario usuario)
        {
            _dbContext.Usuarios.Add(usuario);
            _dbContext.SaveChanges();

            return usuario.Id;
        }

    }
}
