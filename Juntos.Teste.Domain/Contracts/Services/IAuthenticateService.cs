using Juntos.Teste.Domain.Models;

namespace Juntos.Teste.Domain.Contracts.Services
{
    public interface IAuthenticateService
    {
        bool IsAuthenticated(TokenRequest request, out string token);
    }
}
