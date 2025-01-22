using Bobs_Corn_Challenge.DataAccess.Repositories;
using Bobs_Corn_Challenge.Entities;
using Bobs_Corn_Challenge.Services.Interfaces;

namespace Bobs_Corn_Challenge.Services.Implementations
{
    public class CornService : ICornService
    {
        private readonly ICornRepository _cornRepository;

        public CornService(ICornRepository cornRepository)
        {
            _cornRepository = cornRepository;
        }

        public async Task<(bool IsSuccessful, string Message)> ProcessPurchaseAsync(string clientId)
        {
            var canPurchase = await _cornRepository.ValidatePurchaseAsync(clientId);

            await _cornRepository.RegisterPurchaseAsync(clientId, canPurchase);

            return canPurchase
                ? (true, "Successful purchase! 🌽")
                : (false, "You must wait 1 minute between purchases");
        }

        public async Task<IEnumerable<Corn>> GetClientPurchaseHistoryAsync(string clientId)
        {
            return await _cornRepository.GetClientPurchasesAsync(clientId);
        }
    }
}
