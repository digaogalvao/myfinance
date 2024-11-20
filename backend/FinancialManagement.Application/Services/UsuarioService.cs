using FinancialManagement.Application.Interfaces;
using FinancialManagement.Domain.Entities;
using FinancialManagement.Domain.Interfaces;

namespace FinancialManagement.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<Usuario>> GetUsuarios()
        {
            var usuarios = await _usuarioRepository.GetUsuarios();

            usuarios = usuarios.OrderBy(t => t.Nome);

            return usuarios;
        }

        public async Task<Usuario> GetUsuario(int id)
        {
            return await _usuarioRepository.GetUsuario(id);
        }

        public async Task<Usuario> LoginUsuario(string email, string senha)
        {
            var login = await _usuarioRepository.LoginUsuario(email, senha);

            return login;
        }

        public async Task<int> CreateUsuario(Usuario usuario)
        {
            // Criptografa a senha antes de salvar o usuário
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

            var registerUsuario = new Usuario
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = hashedPassword,
            };

            var usuarioId = await _usuarioRepository.CreateUsuario(registerUsuario);

            return usuarioId;
        }
    }
}
