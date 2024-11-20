using FinancialManagement.Domain.Entities;

namespace FinancialManagement.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetUsuarios();
        Task<Usuario> GetUsuario(int id);
        Task<Usuario> LoginUsuario(string email, string senha);
        Task<int> CreateUsuario(Usuario usuario);
    }
}
