using FinancialManagement.Domain.Entities;
using FinancialManagement.Infrastructure.Context;
using FinancialManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinancialManagement.Tests
{
    public class LancamentoRepositoryTests
    {
        [Fact]
        public async Task GetLancamentos_ShouldReturnLancamentos()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Nome único para cada teste
                .Options;

            using var dbContext = new AppDbContext(dbContextOptions);
            var repository = new LancamentoRepository(dbContext);

            // Seed
            var lancamento1 = new Lancamento { Data_Cadastro = DateTime.Now, Descricao = "Test 1", Valor = 100.00m };
            var lancamento2 = new Lancamento { Data_Cadastro = DateTime.Now, Descricao = "Test 2", Valor = 200.00m };
            dbContext.Lancamentos.AddRange(lancamento1, lancamento2);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await repository.GetLancamentos();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetLancamento_ShouldReturnLancamentoById()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetLancamento_ShouldReturnLancamentoById")
                .Options;

            using var dbContext = new AppDbContext(dbContextOptions);
            var repository = new LancamentoRepository(dbContext);

            // Seed the in-memory database with a sample Lancamento
            var lancamento = new Lancamento { Id = 1, Data_Cadastro = DateTime.Now, Descricao = "Test Lancamento", Valor = 100.00m };
            dbContext.Lancamentos.Add(lancamento);
            dbContext.SaveChanges();

            // Act
            var result = await repository.GetLancamentoById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(lancamento.Id, result.Id);
        }

        [Fact]
        public async Task CreateLancamento_ShouldCreateAndReturnId()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var dbContext = new AppDbContext(dbContextOptions);
            var repository = new LancamentoRepository(dbContext);

            var lancamento = new Lancamento { Data_Cadastro = DateTime.Now, Descricao = "New Lancamento", Valor = 500.00m };

            // Act
            var resultId = await repository.CreateLancamento(lancamento);

            // Assert
            Assert.NotEqual(0, resultId);
            var createdLancamento = await dbContext.Lancamentos.FindAsync(resultId);
            Assert.NotNull(createdLancamento);
            Assert.Equal(lancamento.Descricao, createdLancamento.Descricao);
        }

        [Fact]
        public async Task UpdateLancamento_ShouldUpdateLancamento()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UpdateLancamento_ShouldUpdateLancamento")
                .Options;

            using var dbContext = new AppDbContext(dbContextOptions);
            var repository = new LancamentoRepository(dbContext);

            var lancamento = new Lancamento { Id = 1, Data_Cadastro = DateTime.Now, Descricao = "Test Lancamento", Valor = 100.00m };
            dbContext.Lancamentos.Add(lancamento);
            dbContext.SaveChanges();

            // Modify some properties
            lancamento.Descricao = "Updated Lancamento";
            lancamento.Valor = 150.00m;

            // Act
            await repository.UpdateLancamento(lancamento);

            // Assert
            var updatedLancamento = dbContext.Lancamentos.Find(1);
            Assert.NotNull(updatedLancamento);
            Assert.Equal(lancamento.Descricao, updatedLancamento.Descricao);
            Assert.Equal(lancamento.Valor, updatedLancamento.Valor);
        }

        [Fact]
        public async Task DeleteLancamento_ShouldDeleteLancamento()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var dbContext = new AppDbContext(dbContextOptions);
            var repository = new LancamentoRepository(dbContext);

            var lancamento = new Lancamento { Data_Cadastro = DateTime.Now, Descricao = "To Delete", Valor = 300.00m };
            dbContext.Lancamentos.Add(lancamento);
            await dbContext.SaveChangesAsync();

            // Act
            await repository.DeleteLancamento(lancamento);

            // Assert
            var deletedLancamento = await dbContext.Lancamentos.FindAsync(lancamento.Id);
            Assert.Null(deletedLancamento);
        }

        [Fact]
        public async Task DeleteLancamento_ShouldThrowException_WhenLancamentoDoesNotExist()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var dbContext = new AppDbContext(dbContextOptions);
            var repository = new LancamentoRepository(dbContext);

            var lancamento = new Lancamento { Id = 999, Data_Cadastro = DateTime.Now, Descricao = "Nonexistent", Valor = 0.00m };

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => repository.DeleteLancamento(lancamento));
        }

        [Fact]
        public async Task GetFluxoCaixa_ShouldReturnLancamentosWithinDateRange()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var dbContext = new AppDbContext(dbContextOptions);
            var repository = new LancamentoRepository(dbContext);

            var lancamento1 = new Lancamento { Id_Usuario = 1, Data_Pagamento = new DateTime(2023, 11, 1), Descricao = "Test 1", Valor = 100.00m };
            var lancamento2 = new Lancamento { Id_Usuario = 1, Data_Pagamento = new DateTime(2023, 11, 15), Descricao = "Test 2", Valor = 200.00m };
            var lancamento3 = new Lancamento { Id_Usuario = 1, Data_Pagamento = new DateTime(2023, 12, 1), Descricao = "Test 3", Valor = 300.00m };

            dbContext.Lancamentos.AddRange(lancamento1, lancamento2, lancamento3);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await repository.GetFluxoCaixa(1, new DateTime(2023, 11, 1), new DateTime(2023, 11, 30));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, l => l.Descricao == "Test 1");
            Assert.Contains(result, l => l.Descricao == "Test 2");
        }


    }

}
