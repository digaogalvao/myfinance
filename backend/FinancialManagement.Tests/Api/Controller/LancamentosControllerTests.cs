using FinancialManagement.Api.Controllers;
using FinancialManagement.Application.Interfaces;
using FinancialManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FinancialManagement.Tests
{
    public class LancamentosControllerTests
    {
        [Fact]
        public async Task Index_ShouldReturnListOfLancamentos()
        {
            // Arrange
            var lancamentosMock = new List<Lancamento>
            {
                new Lancamento { Id = 1, Descricao = "Lancamento 1", Valor = 100 },
                new Lancamento { Id = 2, Descricao = "Lancamento 2", Valor = 150 },
            };

            var lancamentoServiceMock = new Mock<ILancamentoService>();
            lancamentoServiceMock.Setup(service => service.GetLancamentos()).ReturnsAsync(lancamentosMock);

            var controller = new LancamentosController(lancamentoServiceMock.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var lancamentos = Assert.IsAssignableFrom<IEnumerable<Lancamento>>(okResult.Value);
            Assert.Equal(2, lancamentos.Count());
        }

        [Fact]
        public async Task Index_ShouldReturnNotFound_WhenNoLancamentos()
        {
            // Arrange
            var lancamentoServiceMock = new Mock<ILancamentoService>();
            lancamentoServiceMock.Setup(service => service.GetLancamentos()).ReturnsAsync((IEnumerable<Lancamento>)null);

            var controller = new LancamentosController(lancamentoServiceMock.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Nenhum lançamento cadastrado", notFoundResult.Value);
        }

        [Fact]
        public async Task Details_WithValidId_ShouldReturnLancamento()
        {
            // Arrange
            var lancamentoMock = new Lancamento { Id = 1, Descricao = "Lancamento 1", Valor = 100 };
            var lancamentoServiceMock = new Mock<ILancamentoService>();
            lancamentoServiceMock.Setup(service => service.GetLancamentoById(1)).ReturnsAsync(lancamentoMock);

            var controller = new LancamentosController(lancamentoServiceMock.Object);

            // Act
            var result = await controller.Details(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var lancamento = Assert.IsAssignableFrom<Lancamento>(okResult.Value);
            Assert.Equal(lancamentoMock.Id, lancamento.Id);
        }

        [Fact]
        public async Task Details_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var lancamentoServiceMock = new Mock<ILancamentoService>();
            lancamentoServiceMock.Setup(service => service.GetLancamentoById(99)).ReturnsAsync((Lancamento)null);

            var controller = new LancamentosController(lancamentoServiceMock.Object);

            // Act
            var result = await controller.Details(99);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Lançamento com id= 99 não encontrado", notFoundResult.Value);
        }

        [Fact]
        public async Task DetailsByUser_ShouldReturnListOfLancamentos()
        {
            // Arrange
            var lancamentosMock = new List<Lancamento>
            {
                new Lancamento { Id = 1, Descricao = "Lancamento 1", Id_Usuario = 1, Valor = 100 },
                new Lancamento { Id = 2, Descricao = "Lancamento 2", Id_Usuario = 1, Valor = 150 },
            };

            var lancamentoServiceMock = new Mock<ILancamentoService>();
            lancamentoServiceMock.Setup(service => service.GetLancamentosByUsuario(1)).ReturnsAsync(lancamentosMock);

            var controller = new LancamentosController(lancamentoServiceMock.Object);

            // Act
            var result = await controller.DetailsByUser(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var lancamentos = Assert.IsAssignableFrom<IEnumerable<Lancamento>>(okResult.Value);
            Assert.Equal(2, lancamentos.Count());
        }

        [Fact]
        public void Create_ShouldReturnOkResult()
        {
            // Arrange
            var lancamento = new Lancamento { Id = 1, Descricao = "Nova Lancamento", Valor = 200 };
            var lancamentoServiceMock = new Mock<ILancamentoService>();
            var controller = new LancamentosController(lancamentoServiceMock.Object);

            // Act
            var result = controller.Create(lancamento);

            // Assert
            Assert.IsType<OkResult>(result);
            lancamentoServiceMock.Verify(service => service.CreateLancamento(It.IsAny<Lancamento>()), Times.Once);
        }

        [Fact]
        public async Task Edit_WithValidId_ShouldReturnUpdatedLancamento()
        {
            // Arrange
            var lancamento = new Lancamento { Id = 1, Descricao = "Atualizado", Valor = 200 };
            var lancamentoServiceMock = new Mock<ILancamentoService>();
            lancamentoServiceMock.Setup(service => service.GetLancamentoById(1)).ReturnsAsync(lancamento);
            lancamentoServiceMock.Setup(service => service.UpdateLancamento(lancamento)).Returns(Task.CompletedTask);

            var controller = new LancamentosController(lancamentoServiceMock.Object);

            // Act
            var result = await controller.Edit(1, lancamento);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedLancamento = Assert.IsAssignableFrom<Lancamento>(okResult.Value);
            Assert.Equal(lancamento.Id, updatedLancamento.Id);
        }

        [Fact]
        public async Task Delete_WithValidId_ShouldReturnDeletedLancamento()
        {
            // Arrange
            var lancamento = new Lancamento { Id = 1, Descricao = "Excluir", Valor = 100 };
            var lancamentoServiceMock = new Mock<ILancamentoService>();
            lancamentoServiceMock.Setup(service => service.GetLancamentoById(1)).ReturnsAsync(lancamento);
            lancamentoServiceMock.Setup(service => service.DeleteLancamento(1)).Returns(Task.CompletedTask);

            var controller = new LancamentosController(lancamentoServiceMock.Object);

            // Act
            var result = await controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var deletedLancamento = Assert.IsAssignableFrom<Lancamento>(okResult.Value);
            Assert.Equal(lancamento.Id, deletedLancamento.Id);
        }

        [Fact]
        public async Task GetFluxoCaixa_ShouldReturnResumo()
        {
            // Arrange
            var fluxoCaixaMock = new FluxoCaixa { SaldoFinal = 1000 };
            var lancamentoServiceMock = new Mock<ILancamentoService>();
            lancamentoServiceMock.Setup(service => service.GetFluxoCaixa(1, null, null, null, null)).ReturnsAsync(fluxoCaixaMock);

            var controller = new LancamentosController(lancamentoServiceMock.Object);

            // Act
            var result = await controller.GetFluxoCaixa(1, null, null, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var fluxoCaixa = Assert.IsAssignableFrom<FluxoCaixa>(okResult.Value);
            Assert.Equal(1000, fluxoCaixa.SaldoFinal);
        }
    }
}
