using FinancialManagement.Domain.Entities;
using FinancialManagement.Domain.Interfaces;
using FinancialManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FinancialManagement.Infrastructure.Repositories
{
    public class LancamentoRepository : ILancamentoRepository
    {
        private readonly AppDbContext _dbContext;

        public LancamentoRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Lancamento>> GetLancamentos()
        {
            return await _dbContext.Lancamentos.ToListAsync();
        }

        public async Task<Lancamento> GetLancamentoById(int id)
        {
            var lancamento = await _dbContext.Lancamentos.SingleOrDefaultAsync(c => c.Id == id);

            return lancamento;
        }

        public async Task<IEnumerable<Lancamento>> GetLancamentosByUsuario(int id)
        {
            var lancamentos = await _dbContext.Lancamentos.Where(c => c.Id_Usuario == id).ToListAsync();

            return lancamentos;
        }

        public async Task<int> CreateLancamento(Lancamento lancamento)
        {
            _dbContext.Lancamentos.Add(lancamento);
            _dbContext.SaveChanges();
            return lancamento.Id;
        }

        public async Task UpdateLancamento(Lancamento lancamento)
        {
            _dbContext.Entry(lancamento).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteLancamento(Lancamento lancamento)
        {
            _dbContext.Lancamentos.Remove(lancamento);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Lancamento>> GetFluxoCaixa(int usuarioId, DateTime dataInicial, DateTime dataFinal)
        {
            return await _dbContext.Lancamentos
                .Where(l => l.Id_Usuario == usuarioId &&
                            l.Data_Pagamento >= dataInicial &&
                            l.Data_Pagamento <= dataFinal)
                .ToListAsync();
        }

    }
}
