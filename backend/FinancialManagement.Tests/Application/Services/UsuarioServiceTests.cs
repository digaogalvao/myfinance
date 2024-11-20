using FinancialManagement.Application.Services;
using FinancialManagement.Domain.Entities;
using FinancialManagement.Domain.Interfaces;
using Moq;

namespace FinancialManagement.Tests
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepository;
        private readonly UsuarioService _usuarioService;

        public UsuarioServiceTests()
        {
            _mockUsuarioRepository = new Mock<IUsuarioRepository>();
            _usuarioService = new UsuarioService(_mockUsuarioRepository.Object);
        }

        [Fact]
        public async Task GetUsuarios_ShouldReturnOrderedUsuarios()
        {
            // Arrange
            var usuarios = new List<Usuario>
            {
                new Usuario { Id = 2, Nome = "Carlos", Email = "carlos@example.com", Senha = "12345" },
                new Usuario { Id = 1, Nome = "Ana", Email = "ana@example.com", Senha = "12345" },
            };

            _mockUsuarioRepository.Setup(repo => repo.GetUsuarios())
                .ReturnsAsync(usuarios);

            // Act
            var result = await _usuarioService.GetUsuarios();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Ana", result.First().Nome); // Verifica se está ordenado por nome
        }

        [Fact]
        public async Task GetUsuario_ShouldReturnUsuarioById()
        {
            // Arrange
            var usuario = new Usuario { Id = 1, Nome = "Ana", Email = "ana@example.com", Senha = "12345" };

            _mockUsuarioRepository.Setup(repo => repo.GetUsuario(1))
                .ReturnsAsync(usuario);

            // Act
            var result = await _usuarioService.GetUsuario(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(usuario.Id, result.Id);
            Assert.Equal(usuario.Nome, result.Nome);
        }

        [Fact]
        public async Task LoginUsuario_ShouldReturnUsuarioIfValid()
        {
            // Arrange
            var email = "ana@example.com";
            var senha = "12345";

            var usuario = new Usuario { Id = 1, Nome = "Ana", Email = email, Senha = BCrypt.Net.BCrypt.HashPassword(senha) };

            _mockUsuarioRepository.Setup(repo => repo.LoginUsuario(email, senha))
                .ReturnsAsync(usuario);

            // Act
            var result = await _usuarioService.LoginUsuario(email, senha);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(usuario.Id, result.Id);
            Assert.Equal(usuario.Nome, result.Nome);
        }

        [Fact]
        public async Task LoginUsuario_ShouldReturnNullIfInvalid()
        {
            // Arrange
            var email = "ana@example.com";
            var senha = "12345";

            _mockUsuarioRepository.Setup(repo => repo.LoginUsuario(email, senha))
                .ReturnsAsync((Usuario)null);

            // Act
            var result = await _usuarioService.LoginUsuario(email, senha);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateUsuario_ShouldReturnUsuarioId()
        {
            // Arrange
            var usuario = new Usuario
            {
                Nome = "Ana",
                Email = "ana@example.com",
                Senha = "12345",
            };

            _mockUsuarioRepository.Setup(repo => repo.CreateUsuario(It.IsAny<Usuario>()))
                .ReturnsAsync(1);

            // Act
            var result = await _usuarioService.CreateUsuario(usuario);

            // Assert
            Assert.Equal(1, result);
            _mockUsuarioRepository.Verify(repo => repo.CreateUsuario(It.Is<Usuario>(
                u => u.Email == usuario.Email && BCrypt.Net.BCrypt.Verify(usuario.Senha, u.Senha))), Times.Once);
        }
    }
}
