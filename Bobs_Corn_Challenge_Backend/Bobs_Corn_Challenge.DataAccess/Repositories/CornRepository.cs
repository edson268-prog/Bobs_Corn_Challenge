using Bobs_Corn_Challenge.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobs_Corn_Challenge.DataAccess.Repositories
{
    public class CornRepository : ICornRepository
    {
        private readonly ChallengeDbContext _context;

        public CornRepository(ChallengeDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ValidatePurchaseAsync(string clientId)
        {
            var lastPurchase = await _context.CornPurchases
                .Where(c => c.ClientId == clientId && c.IsSuccessful)
                .OrderByDescending(c => c.PurchaseTime)
                .FirstOrDefaultAsync();

            if (lastPurchase == null)
                return true;

            var timeSinceLastPurchase = DateTime.UtcNow - lastPurchase.PurchaseTime;
            return timeSinceLastPurchase.TotalSeconds >= 60;
        }

        public async Task<Corn> RegisterPurchaseAsync(string clientId, bool isSuccessful)
        {
            var purchase = new Corn
            {
                ClientId = clientId,
                PurchaseTime = DateTime.UtcNow,
                IsSuccessful = isSuccessful
            };

            _context.CornPurchases.Add(purchase);
            await _context.SaveChangesAsync();
            return purchase;
        }

        public async Task<IEnumerable<Corn>> GetClientPurchasesAsync(string clientId)
        {
            return await _context.CornPurchases
                .Where(c => c.ClientId == clientId)
                .OrderByDescending(c => c.PurchaseTime)
                .ToListAsync();
        }
    }
}
