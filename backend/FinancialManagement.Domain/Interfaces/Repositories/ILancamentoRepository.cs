using FinancialManagement.Domain.Entities;

namespace FinancialManagement.Domain.Interfaces
{
    public interface ILancamentoRepository
    {
        Task<IEnumerable<Lancamento>> GetLancamentos();
        Task<Lancamento> GetLancamentoById(int id);
        Task<IEnumerable<Lancamento>> GetLancamentosByUsuario(int id);
        Task<int> CreateLancamento(Lancamento lancamento);
        Task UpdateLancamento(Lancamento lancamento);
        Task DeleteLancamento(Lancamento lancamento);
        Task<IEnumerable<Lancamento>> GetFluxoCaixa(int usuarioId, DateTime dataInicial, DateTime dataFinal);
    }
}
