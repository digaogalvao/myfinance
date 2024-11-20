using FinancialManagement.Domain.Entities;

namespace FinancialManagement.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(Usuario usuario);
    }
}
