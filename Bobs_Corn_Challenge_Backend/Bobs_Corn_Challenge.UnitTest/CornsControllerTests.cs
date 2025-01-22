using Bobs_Corn_Challenge.API.Controllers;
using Bobs_Corn_Challenge.API.Exceptions;
using Bobs_Corn_Challenge.Entities;
using Bobs_Corn_Challenge.Entities.dto.Request;
using Bobs_Corn_Challenge.Entities.dto.Response;
using Bobs_Corn_Challenge.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobs_Corn_Challenge.UnitTest
{
    public class CornsControllerTests
    {
        private readonly Mock<ICornService> _mockService;
        private readonly Mock<ILogger<CornsController>> _mockLogger;
        private readonly CornsController _controller;

        public CornsControllerTests()
        {
            _mockService = new Mock<ICornService>();
            _mockLogger = new Mock<ILogger<CornsController>>();
            _controller = new CornsController(_mockService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task PurchaseCorn_WhenSuccessful_ReturnsOkResult()
        {
            //Set a successful mock and and check if the response is successful

            // Arrange
            var request = new PurchaseCornRequestDto { ClientId = "client-746219" };
            _mockService.Setup(s => s.ProcessPurchaseAsync(request.ClientId))
                .ReturnsAsync((true, "Successful purchase! 🌽"));

            // Act
            var result = await _controller.PurchaseCorn(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<PurchaseCornResponseDto>(okResult.Value);
            Assert.True(response.IsSuccessful);
        }

        [Fact]
        public async Task PurchaseCorn_WhenRateLimited_ThrowsRateLimitException()
        {
            //Set the result status and message to get a rate limit exception

            // Arrange
            var request = new PurchaseCornRequestDto { ClientId = "client-746219" };
            _mockService.Setup(s => s.ProcessPurchaseAsync(request.ClientId))
                .ReturnsAsync((false, "You must wait 1 minute between purchases"));

            // Act & Assert
            await Assert.ThrowsAsync<RateLimitExceededException>(() =>
                _controller.PurchaseCorn(request));
        }

        [Fact]
        public async Task GetPurchaseHistory_ReturnsCorrectHistory()
        {
            //Set a mock result for the purchase history request and check if the response is correct

            // Arrange
            var clientId = "client-746219";
            var expectedHistory = new List<Corn>
        {
            new() { Id = 1, ClientId = clientId, IsSuccessful = true }
        };

            _mockService.Setup(s => s.GetClientPurchaseHistoryAsync(clientId))
                .ReturnsAsync(expectedHistory);

            // Act
            var result = await _controller.GetPurchaseHistory(clientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var history = Assert.IsType<List<Corn>>(okResult.Value);
            Assert.Single(history);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetPurchaseHistory_WithInvalidClientId_ThrowsOtherException(string clientId)
        {
            // Set invalid clients ids and check if the response throws an OtherException

            // Act & Assert
            await Assert.ThrowsAsync<OtherException>(() =>
                _controller.GetPurchaseHistory(clientId));
        }

        [Fact]
        public async Task GetClientStats_ReturnsCorrectStats()
        {
            //Set a mock result for the client stats request and check if the response is correct

            // Arrange
            var clientId = "client-746219";
            var purchases = new List<Corn>
        {
            new() { ClientId = clientId, IsSuccessful = true },
            new() { ClientId = clientId, IsSuccessful = false },
            new() { ClientId = clientId, IsSuccessful = true }
        };

            _mockService.Setup(s => s.GetClientPurchaseHistoryAsync(clientId))
                .ReturnsAsync(purchases);

            // Act
            var result = await _controller.GetClientStats(clientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var stats = Assert.IsType<ClientHistoryResponseDto>(okResult.Value);
            Assert.Equal(3, stats.TotalPurchases);
            Assert.Equal(2, stats.SuccessfulPurchases);
        }
    }
}
