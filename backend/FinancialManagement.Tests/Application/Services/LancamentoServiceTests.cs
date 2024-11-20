using FinancialManagement.Application.Services;
using FinancialManagement.Domain.Entities;
using FinancialManagement.Domain.Enum;
using FinancialManagement.Domain.Interfaces;
using Moq;

namespace FinancialManagement.Tests
{
    public class LancamentoServiceTests
    {
        [Fact]
        public async Task GetLancamentos_ShouldReturnOrderedLancamentos()
        {
            // Arrange
            var lancamentos = new List<Lancamento>
            {
                new Lancamento { Id = 1, Data_Pagamento = DateTime.Now.AddDays(-1) },
                new Lancamento { Id = 2, Data_Pagamento = DateTime.Now },
                new Lancamento { Id = 3, Data_Pagamento = DateTime.Now.AddDays(-2) }
            };

            var lancamentoRepositoryMock = new Mock<ILancamentoRepository>();
            lancamentoRepositoryMock.Setup(repo => repo.GetLancamentos()).ReturnsAsync(lancamentos);

            var lancamentoService = new LancamentoService(lancamentoRepositoryMock.Object);

            // Act
            var result = await lancamentoService.GetLancamentos();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Equal(2, result.First().Id); // Verifica que está ordenado por Data_Pagamento decrescente
        }

        [Fact]
        public async Task GetLancamentoById_ShouldReturnLancamento()
        {
            // Arrange
            var lancamentoId = 1;
            var lancamento = new Lancamento { Id = lancamentoId, Data_Pagamento = DateTime.Now };

            var lancamentoRepositoryMock = new Mock<ILancamentoRepository>();
            lancamentoRepositoryMock.Setup(repo => repo.GetLancamentoById(lancamentoId)).ReturnsAsync(lancamento);

            var lancamentoService = new LancamentoService(lancamentoRepositoryMock.Object);

            // Act
            var result = await lancamentoService.GetLancamentoById(lancamentoId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(lancamentoId, result.Id);
        }

        [Fact]
        public async Task CreateLancamento_ShouldReturnLancamentoId()
        {
            // Arrange
            var lancamentoToCreate = new Lancamento
            {
                Id_Usuario = 1,
                Data_Pagamento = DateTime.Now,
                Descricao = "Novo Lançamento",
                Valor = 500.00m,
                Tipo = EnumTipoLancamento.Credito
            };

            var lancamentoRepositoryMock = new Mock<ILancamentoRepository>();
            lancamentoRepositoryMock.Setup(repo => repo.CreateLancamento(It.IsAny<Lancamento>())).ReturnsAsync(1);

            var lancamentoService = new LancamentoService(lancamentoRepositoryMock.Object);

            // Act
            var result = await lancamentoService.CreateLancamento(lancamentoToCreate);

            // Assert
            Assert.Equal(1, result);
            lancamentoRepositoryMock.Verify(repo => repo.CreateLancamento(It.IsAny<Lancamento>()), Times.Once);
        }

        [Fact]
        public async Task UpdateLancamento_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var lancamentoToUpdate = new Lancamento
            {
                Id = 1,
                Data_Pagamento = DateTime.Now,
                Descricao = "Atualização Lançamento",
                Valor = 200.00m,
                Tipo = EnumTipoLancamento.Debito
            };

            var lancamentoRepositoryMock = new Mock<ILancamentoRepository>();
            lancamentoRepositoryMock.Setup(repo => repo.UpdateLancamento(It.IsAny<Lancamento>()));

            var lancamentoService = new LancamentoService(lancamentoRepositoryMock.Object);

            // Act
            await lancamentoService.UpdateLancamento(lancamentoToUpdate);

            // Assert
            lancamentoRepositoryMock.Verify(repo => repo.UpdateLancamento(lancamentoToUpdate), Times.Once);
        }

        [Fact]
        public async Task DeleteLancamento_ShouldDeleteLancamentoWhenFound()
        {
            // Arrange
            var lancamentoId = 1;
            var lancamentoToDelete = new Lancamento { Id = lancamentoId };

            var lancamentoRepositoryMock = new Mock<ILancamentoRepository>();
            lancamentoRepositoryMock.Setup(repo => repo.GetLancamentoById(lancamentoId)).ReturnsAsync(lancamentoToDelete);
            lancamentoRepositoryMock.Setup(repo => repo.DeleteLancamento(It.IsAny<Lancamento>()));

            var lancamentoService = new LancamentoService(lancamentoRepositoryMock.Object);

            // Act
            await lancamentoService.DeleteLancamento(lancamentoId);

            // Assert
            lancamentoRepositoryMock.Verify(repo => repo.DeleteLancamento(lancamentoToDelete), Times.Once);
        }

        [Fact]
        public async Task DeleteLancamento_ShouldThrowExceptionWhenLancamentoNotFound()
        {
            // Arrange
            var lancamentoId = 99;

            var lancamentoRepositoryMock = new Mock<ILancamentoRepository>();
            lancamentoRepositoryMock.Setup(repo => repo.GetLancamentoById(lancamentoId)).ReturnsAsync((Lancamento)null);

            var lancamentoService = new LancamentoService(lancamentoRepositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() => lancamentoService.DeleteLancamento(lancamentoId));
        }

        [Fact]
        public async Task GetFluxoCaixa_ShouldCalculateCorrectValues()
        {
            // Arrange
            var usuarioId = 1;
            var dataInicial = new DateTime(2024, 1, 1);
            var dataFinal = new DateTime(2024, 1, 31);

            var lancamentos = new List<Lancamento>
            {
                new Lancamento { Id = 1, Tipo = EnumTipoLancamento.Credito, Valor = 100.00m },
                new Lancamento { Id = 2, Tipo = EnumTipoLancamento.Debito, Valor = 50.00m },
                new Lancamento { Id = 3, Tipo = EnumTipoLancamento.Credito, Valor = 200.00m }
            };

            var lancamentoRepositoryMock = new Mock<ILancamentoRepository>();
            lancamentoRepositoryMock
                .Setup(repo => repo.GetFluxoCaixa(usuarioId, dataInicial, dataFinal))
                .ReturnsAsync(lancamentos);

            var lancamentoService = new LancamentoService(lancamentoRepositoryMock.Object);

            // Act
            var fluxoCaixa = await lancamentoService.GetFluxoCaixa(usuarioId, dataInicial, dataFinal, null, null);

            // Assert
            Assert.NotNull(fluxoCaixa);
            Assert.Equal(50.00m, fluxoCaixa.TotalGastos);
            Assert.Equal(300.00m, fluxoCaixa.TotalRecebidos);
            Assert.Equal(250.00m, fluxoCaixa.SaldoFinal);
        }
    }
}
