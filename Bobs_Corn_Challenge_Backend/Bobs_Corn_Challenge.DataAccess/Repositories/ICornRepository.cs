using Bobs_Corn_Challenge.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobs_Corn_Challenge.DataAccess.Repositories
{
    public interface ICornRepository
    {
        Task<bool> ValidatePurchaseAsync(string clientId);
        Task<Corn> RegisterPurchaseAsync(string clientId, bool isSuccessful);
        Task<IEnumerable<Corn>> GetClientPurchasesAsync(string clientId);
    }
}
