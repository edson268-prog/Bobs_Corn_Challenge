using Bobs_Corn_Challenge.DataAccess.Repositories;
using Bobs_Corn_Challenge.DataAccess;
using Bobs_Corn_Challenge.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Corn_Challenge.UnitTest
{
    public class CornRepositoryTests
    {
        private readonly DbContextOptions<ChallengeDbContext> _options;
        private readonly ChallengeDbContext _context;
        private readonly CornRepository _repository;

        public CornRepositoryTests()
        {
            // Setup in-memory database

            _options = new DbContextOptionsBuilder<ChallengeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ChallengeDbContext(_options);
            _repository = new CornRepository(_context);
        }

        [Fact]
        public async Task ValidatePurchase_WithNoPreviousPurchases_ReturnsTrue()
        {
            // Set a new client with no previous purchases and check if the purchase is valid

            // Arrange
            var clientId = "client-000001";

            // Act
            var result = await _repository.ValidatePurchaseAsync(clientId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ValidatePurchase_WithRecentPurchase_ReturnsFalse()
        {
            // Set a recent purchase and check if the purchase result is invalid

            // Arrange
            var clientId = "client-000001";
            var recentPurchase = new Corn
            {
                ClientId = clientId,
                PurchaseTime = DateTime.UtcNow,
                IsSuccessful = true
            };
            _context.CornPurchases.Add(recentPurchase);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ValidatePurchaseAsync(clientId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidatePurchase_WithOldPurchase_ReturnsTrue()
        {
            // Set an old purchase with 2 minutes of diference and check if the purchase is valid

            // Arrange
            var clientId = "client-746219";
            var oldPurchase = new Corn
            {
                ClientId = clientId,
                PurchaseTime = DateTime.UtcNow.AddMinutes(-2),
                IsSuccessful = true
            };
            _context.CornPurchases.Add(oldPurchase);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ValidatePurchaseAsync(clientId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RegisterPurchase_SavesPurchaseCorrectly()
        {
            // Set a new purchase and check if it is saved correctly

            // Arrange
            var clientId = "client-746219";
            var isSuccessful = true;

            // Act
            var result = await _repository.RegisterPurchaseAsync(clientId, isSuccessful);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(clientId, result.ClientId);
            Assert.Equal(isSuccessful, result.IsSuccessful);
            Assert.True(_context.CornPurchases.Any(p => p.ClientId == clientId));
        }
    }
}
