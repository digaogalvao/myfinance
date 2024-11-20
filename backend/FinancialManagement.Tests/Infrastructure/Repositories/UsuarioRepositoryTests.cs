using FinancialManagement.Domain.Entities;
using FinancialManagement.Infrastructure.Context;
using FinancialManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FinancialManagement.Tests
{
    public class UsuarioRepositoryTests
    {
        private readonly AppDbContext _dbContext;
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(options);
            _dbContext.Database.EnsureCreated();

            _usuarioRepository = new UsuarioRepository(_dbContext);
        }

        [Fact]
        public async Task GetUsuarios_ShouldReturnAllUsuarios()
        {
            // Arrange
            _dbContext.Usuarios.Add(new Usuario { Id = 1, Nome = "Ana", Email = "ana@example.com", Senha = "12345" });
            _dbContext.Usuarios.Add(new Usuario { Id = 2, Nome = "Carlos", Email = "carlos@example.com", Senha = "12345" });
            _dbContext.SaveChanges();

            // Act
            var usuarios = await _usuarioRepository.GetUsuarios();

            // Assert
            Assert.NotNull(usuarios);
            Assert.Equal(2, usuarios.Count());
        }

        [Fact]
        public async Task GetUsuario_ShouldReturnUsuarioById()
        {
            // Arrange
            var usuario = new Usuario { Id = 1, Nome = "Ana", Email = "ana@example.com", Senha = "12345" };
            _dbContext.Usuarios.Add(usuario);
            _dbContext.SaveChanges();

            // Act
            var result = await _usuarioRepository.GetUsuario(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(usuario.Id, result.Id);
            Assert.Equal(usuario.Nome, result.Nome);
        }

        [Fact]
        public async Task GetUsuario_ShouldReturnNullIfNotFound()
        {
            // Act
            var result = await _usuarioRepository.GetUsuario(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginUsuario_ShouldReturnUsuarioIfCredentialsAreValid()
        {
            // Arrange
            var senha = "senha123";
            var hashedSenha = BCrypt.Net.BCrypt.HashPassword(senha);

            var usuario = new Usuario { Id = 1, Nome = "Ana", Email = "ana@example.com", Senha = hashedSenha };
            _dbContext.Usuarios.Add(usuario);
            _dbContext.SaveChanges();

            // Act
            var result = await _usuarioRepository.LoginUsuario("ana@example.com", senha);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(usuario.Id, result.Id);
            Assert.Equal(usuario.Email, result.Email);
        }

        [Fact]
        public async Task LoginUsuario_ShouldReturnNullIfCredentialsAreInvalid()
        {
            // Arrange
            var senha = "senha123";
            var hashedSenha = BCrypt.Net.BCrypt.HashPassword(senha);

            var usuario = new Usuario { Id = 1, Nome = "Ana", Email = "ana@example.com", Senha = hashedSenha };
            _dbContext.Usuarios.Add(usuario);
            _dbContext.SaveChanges();

            // Act
            var result = await _usuarioRepository.LoginUsuario("ana@example.com", "senhaIncorreta");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateUsuario_ShouldAddUsuarioAndReturnId()
        {
            // Arrange
            var usuario = new Usuario { Nome = "Ana", Email = "ana@example.com", Senha = "12345" };

            // Act
            var result = await _usuarioRepository.CreateUsuario(usuario);

            // Assert
            Assert.Equal(1, result); // O primeiro ID em um banco de dados em memória começa em 1
            var createdUsuario = await _dbContext.Usuarios.FindAsync(result);
            Assert.NotNull(createdUsuario);
            Assert.Equal(usuario.Nome, createdUsuario.Nome);
        }
    }
}
