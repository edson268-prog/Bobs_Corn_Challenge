using Bobs_Corn_Challenge.Entities;

namespace Bobs_Corn_Challenge.Services.Interfaces
{
    public interface ICornService
    {
        Task<(bool IsSuccessful, string Message)> ProcessPurchaseAsync(string clientId);
        Task<IEnumerable<Corn>> GetClientPurchaseHistoryAsync(string clientId);
    }
}
