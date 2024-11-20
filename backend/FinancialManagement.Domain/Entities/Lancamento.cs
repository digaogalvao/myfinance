using FinancialManagement.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialManagement.Domain.Entities
{
    public class Lancamento
    {
        public int Id { get; set; }
        public int Id_Usuario { get; set; }
        public EnumTipoNatureza Natureza { get; set; }
        public EnumTipoLancamento Tipo { get; set; }
        public string? Descricao { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Valor { get; set; }
        public DateTime Data_Pagamento { get; set; }
        public DateTime Data_Cadastro { get; set; }
        public string? Observacao { get; set; }
    }
}
