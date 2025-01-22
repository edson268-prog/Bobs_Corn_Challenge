using Bobs_Corn_Challenge.DataAccess.Repositories;
using Bobs_Corn_Challenge.Entities;
using Bobs_Corn_Challenge.Services.Implementations;
using Moq;

namespace Bobs_Corn_Challenge.UnitTest;

public class CornServiceTests
{
    private readonly Mock<ICornRepository> _mockRepository;
    private readonly CornService _service;

    public CornServiceTests()
    {
        _mockRepository = new Mock<ICornRepository>();
        _service = new CornService(_mockRepository.Object);
    }

    [Fact]
    public async Task ProcessPurchase_WhenCanPurchase_ReturnsSuccessful()
    {
        // Set a successful purchase and check if the response message is successful

        // Arrange
        var clientId = "client-746219";
        _mockRepository.Setup(r => r.ValidatePurchaseAsync(clientId))
            .ReturnsAsync(true);
        _mockRepository.Setup(r => r.RegisterPurchaseAsync(clientId, true))
            .ReturnsAsync(new Corn { ClientId = clientId, IsSuccessful = true });

        // Act
        var result = await _service.ProcessPurchaseAsync(clientId);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Contains("Successful purchase", result.Message);
        _mockRepository.Verify(r => r.RegisterPurchaseAsync(clientId, true), Times.Once);
    }

    [Fact]
    public async Task ProcessPurchase_WhenCannotPurchase_ReturnsUnsuccessful()
    {
        // Set a failed purchase and check if the response message is unsuccessful

        // Arrange
        var clientId = "client-746219";
        _mockRepository.Setup(r => r.ValidatePurchaseAsync(clientId))
            .ReturnsAsync(false);
        _mockRepository.Setup(r => r.RegisterPurchaseAsync(clientId, false))
            .ReturnsAsync(new Corn { ClientId = clientId, IsSuccessful = false });

        // Act
        var result = await _service.ProcessPurchaseAsync(clientId);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Contains("You must wait 1 minute", result.Message);
        _mockRepository.Verify(r => r.RegisterPurchaseAsync(clientId, false), Times.Once);
    }

    [Fact]
    public async Task GetClientPurchaseHistory_ReturnsCorrectHistory()
    {
        // Set a mock result for the purchase history request and check if the response is the expected and called just one time

        // Arrange
        var clientId = "client-746219";
        var expectedPurchases = new List<Corn>
        {
            new() { Id = 1, ClientId = clientId, IsSuccessful = true },
            new() { Id = 2, ClientId = clientId, IsSuccessful = false }
        };

        _mockRepository.Setup(r => r.GetClientPurchasesAsync(clientId))
            .ReturnsAsync(expectedPurchases);

        // Act
        var result = await _service.GetClientPurchaseHistoryAsync(clientId);

        // Assert
        Assert.Equal(expectedPurchases, result);
        _mockRepository.Verify(r => r.GetClientPurchasesAsync(clientId), Times.Once);
    }
}