using FinancialManagement.Domain.Entities;

namespace FinancialManagement.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetUsuarios();
        Task<Usuario> GetUsuario(int id);
        Task<Usuario> LoginUsuario(string email, string senha);
        Task<int> CreateUsuario(Usuario usuario);
    }
}
