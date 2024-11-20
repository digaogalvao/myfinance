using FinancialManagement.Domain.Entities;

namespace FinancialManagement.Application.Interfaces
{
    public interface ILancamentoService
    {
        Task<IEnumerable<Lancamento>> GetLancamentos();
        Task<Lancamento> GetLancamentoById(int id);
        Task<IEnumerable<Lancamento>> GetLancamentosByUsuario(int id);
        Task<int> CreateLancamento(Lancamento lancamento);
        Task UpdateLancamento(Lancamento lancamento);
        Task DeleteLancamento(int id);
        Task<FluxoCaixa> GetFluxoCaixa(int usuarioId, DateTime? dataInicial, DateTime? dataFinal, int? mes, int? ano);
    }
}
