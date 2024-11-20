using FinancialManagement.Api.Controllers;
using FinancialManagement.Api.ViewModels;
using FinancialManagement.Application.Interfaces;
using FinancialManagement.Domain.Entities;
using FinancialManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FinancialManagement.Tests
{
    public class UsuariosControllerTests
    {
        private readonly Mock<IUsuarioService> _mockUsuarioService;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly UsuariosController _controller;

        public UsuariosControllerTests()
        {
            _mockUsuarioService = new Mock<IUsuarioService>();
            _mockTokenService = new Mock<ITokenService>();
            _controller = new UsuariosController(_mockUsuarioService.Object, _mockTokenService.Object);
        }

        [Fact]
        public async Task Index_ShouldReturnOkWithUsuarios_WhenUsuariosExist()
        {
            // Arrange
            var usuarios = new List<Usuario>
            {
                new Usuario { Id = 1, Nome = "Ana", Email = "ana@example.com" },
                new Usuario { Id = 2, Nome = "Carlos", Email = "carlos@example.com" }
            };

            _mockUsuarioService.Setup(service => service.GetUsuarios())
                .ReturnsAsync(usuarios);

            // Act
            var result = await _controller.Index();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsuarios = Assert.IsAssignableFrom<IEnumerable<Usuario>>(okResult.Value);
            Assert.Equal(2, returnedUsuarios.Count());
        }

        [Fact]
        public async Task Index_ShouldReturnNotFound_WhenNoUsuariosExist()
        {
            // Arrange
            _mockUsuarioService.Setup(service => service.GetUsuarios())
                .ReturnsAsync((IEnumerable<Usuario>)null);

            // Act
            var result = await _controller.Index();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Nenhum usuário cadastrado", notFoundResult.Value);
        }

        [Fact]
        public async Task Details_ShouldReturnOkWithUsuario_WhenUsuarioExists()
        {
            // Arrange
            var usuario = new Usuario { Id = 1, Nome = "Ana", Email = "ana@example.com" };

            _mockUsuarioService.Setup(service => service.GetUsuario(1))
                .ReturnsAsync(usuario);

            // Act
            var result = await _controller.Details(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsuario = Assert.IsType<Usuario>(okResult.Value);
            Assert.Equal(usuario.Id, returnedUsuario.Id);
        }

        [Fact]
        public async Task Details_ShouldReturnNotFound_WhenUsuarioDoesNotExist()
        {
            // Arrange
            _mockUsuarioService.Setup(service => service.GetUsuario(999))
                .ReturnsAsync((Usuario)null);

            // Act
            var result = await _controller.Details(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Usuário com id= 999 não encontrado", notFoundResult.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnOkWithToken_WhenCredentialsAreValid()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            {
                Email = "user@example.com",
                Senha = "password"
            };

            var usuario = new Usuario
            {
                Id = 1,
                Nome = "User Name",
                Email = "user@example.com",
                Senha = "hashedpassword"
            };

            // Simulando o retorno do serviço de login
            _mockUsuarioService.Setup(service => service.LoginUsuario(loginViewModel.Email, loginViewModel.Senha))
                .ReturnsAsync(usuario);

            var token = "mocked_token"; // Simulando o token gerado pelo seu TokenService
            _mockTokenService.Setup(service => service.GenerateToken(usuario)).Returns(token);

            // Act
            var result = await _controller.Login(loginViewModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<LoginResponseViewModel>(okResult.Value);

            Assert.Equal(token, returnValue.Token);
            Assert.Equal(usuario.Id, returnValue.UserId);
            Assert.Equal(usuario.Nome, returnValue.UserName);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var usuario = new LoginViewModel { Email = "ana@example.com", Senha = "wrong-password" };

            _mockUsuarioService.Setup(service => service.LoginUsuario(usuario.Email, usuario.Senha))
                .ReturnsAsync((Usuario)null);

            // Act
            var result = await _controller.Login(usuario);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Email ou senha inválidos", unauthorizedResult.Value);
        }

        [Fact]
        public async Task Create_ShouldReturnOk_WhenUsuarioIsCreatedSuccessfully()
        {
            // Arrange
            var usuario = new Usuario { Nome = "Ana", Email = "ana@example.com", Senha = "12345" };

            _mockUsuarioService.Setup(service => service.CreateUsuario(usuario))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.Create(usuario);  // Aguarde a tarefa ser completada

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var usuario = new Usuario { Nome = "Ana", Email = "ana@example.com", Senha = "12345" };

            // Configura o mock para lançar uma exceção quando CreateUsuario for chamado
            _mockUsuarioService.Setup(service => service.CreateUsuario(usuario))
                .ThrowsAsync(new Exception("Erro ao criar usuário"));

            // Act
            var result = await _controller.Create(usuario);  // Remova o 'await' aqui, já que o método 'Create' não é assíncrono

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);  // Verifica se o retorno foi 'BadRequest'
            Assert.Equal("Erro ao criar usuário", badRequestResult.Value);  // Verifica se a mensagem é a correta
        }

    }
}
