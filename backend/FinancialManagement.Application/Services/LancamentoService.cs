using FinancialManagement.Application.Interfaces;
using FinancialManagement.Domain.Entities;
using FinancialManagement.Domain.Enum;
using FinancialManagement.Domain.Interfaces;

namespace FinancialManagement.Application.Services
{
    public class LancamentoService : ILancamentoService
    {
        private readonly ILancamentoRepository _lancamentoRepository;

        public LancamentoService(ILancamentoRepository lancamentoRepository)
        {
            _lancamentoRepository = lancamentoRepository;
        }

        public async Task<IEnumerable<Lancamento>> GetLancamentos()
        {
            var lancamentos = await _lancamentoRepository.GetLancamentos();

            lancamentos = lancamentos.OrderByDescending(t => t.Data_Pagamento);

            return lancamentos;
        }

        public async Task<Lancamento> GetLancamentoById(int id)
        {
            return await _lancamentoRepository.GetLancamentoById(id);
        }

        public async Task<IEnumerable<Lancamento>> GetLancamentosByUsuario(int id)
        {
            var lancamentos = await _lancamentoRepository.GetLancamentosByUsuario(id);

            lancamentos = lancamentos.OrderByDescending(t => t.Data_Pagamento);

            return lancamentos;
        }

        public async Task<int> CreateLancamento(Lancamento lancamento)
        {
            var createLancamento = new Lancamento
            {
                Id_Usuario = lancamento.Id_Usuario,
                Natureza = lancamento.Natureza,
                Tipo = lancamento.Tipo,
                Descricao = lancamento.Descricao,
                Valor = lancamento.Valor,
                Data_Pagamento = lancamento.Data_Pagamento,
                Data_Cadastro = lancamento.Data_Cadastro,
                Observacao = lancamento.Observacao,
            };

            var lancamentoId = await _lancamentoRepository.CreateLancamento(createLancamento);

            return lancamentoId;
        }

        public async Task UpdateLancamento(Lancamento lancamento)
        {
            if (lancamento is null)
                throw new ApplicationException("Dados inválidos");

            await _lancamentoRepository.UpdateLancamento(lancamento);
        }

        public async Task DeleteLancamento(int id)
        {
            var lancamento = await _lancamentoRepository.GetLancamentoById(id);

            if (lancamento is null)
                throw new ApplicationException("Transação não encontrada");

            await _lancamentoRepository.DeleteLancamento(lancamento);
        }

        /*public async Task<FluxoCaixa> RelatorioDiario(DateTime data)
        {
            var transacoesDoDia = await _lancamentoRepository.RelatorioDiario(data);

            decimal saldoDoDia = transacoesDoDia.Sum(t => t.Tipo == EnumTipoLancamento.Credito ? t.Valor : -t.Valor);

            var relatorioDiario = new FluxoCaixa
            {
                Data = data,
                SaldoDoDia = saldoDoDia
            };

            return relatorioDiario;
        }*/

        public async Task<FluxoCaixa> GetFluxoCaixa(int usuarioId, DateTime? dataInicial, DateTime? dataFinal, int? mes, int? ano)
        {
            // Define o período de filtro
            if (mes.HasValue && ano.HasValue)
            {
                dataInicial = new DateTime(ano.Value, mes.Value, 1);
                dataFinal = dataInicial.Value.AddMonths(1).AddDays(-1);
            }
            else if (!dataInicial.HasValue || !dataFinal.HasValue)
            {
                dataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dataFinal = dataInicial.Value.AddMonths(1).AddDays(-1);
            }

            // Busca os lançamentos no repositório
            var lancamentos = await _lancamentoRepository.GetFluxoCaixa(usuarioId, dataInicial.Value, dataFinal.Value);

            // Ordena os lançamentos por data de pagamento
            lancamentos = lancamentos.OrderByDescending(t => t.Data_Pagamento);

            // Calcula os totais
            var totalGastos = lancamentos.Where(l => l.Tipo == EnumTipoLancamento.Debito).Sum(l => l.Valor);
            var totalRecebidos = lancamentos.Where(l => l.Tipo == EnumTipoLancamento.Credito).Sum(l => l.Valor);
            var saldoFinal = totalRecebidos - totalGastos;

            return new FluxoCaixa
            {
                TotalGastos = totalGastos,
                TotalRecebidos = totalRecebidos,
                SaldoFinal = saldoFinal,
                DataInicial = dataInicial.Value,
                DataFinal = dataFinal.Value
            };
        }

    }
}
