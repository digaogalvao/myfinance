namespace FinancialManagement.Domain.Entities
{
    public class FluxoCaixa
    {
        public decimal TotalGastos { get; set; }
        public decimal TotalRecebidos { get; set; }
        public decimal SaldoFinal { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
    }
}
