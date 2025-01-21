using Microsoft.AspNetCore.Mvc;
using Bobs_Corn_Challenge.Entities;
using Bobs_Corn_Challenge.Services.Interfaces;
using Bobs_Corn_Challenge.API.Exceptions;
using static Bobs_Corn_Challenge.API.Middleware.ExceptionMiddleware;
using Bobs_Corn_Challenge.Entities.dto.Response;
using Bobs_Corn_Challenge.Entities.dto.Request;
using Azure.Core;

namespace Bobs_Corn_Challenge.API.Controllers
{
    [Route(Constants.DefaultRoute)]
    [ApiController]
    public class CornsController : ControllerBase
    {
        private readonly ICornService _cornService;
        private readonly ILogger<CornsController> _logger;

        public CornsController(ICornService cornService, ILogger<CornsController> logger)
        {
            _cornService = cornService;
            _logger = logger;
        }

        [HttpPost("purchase")]
        [ProducesResponseType(typeof(PurchaseCornResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> PurchaseCorn([FromBody] PurchaseCornRequestDto request)
        {
            try
            {
                var (isSuccessful, message) = await _cornService.ProcessPurchaseAsync(request.ClientId);

                if (!isSuccessful)
                {
                    throw new RateLimitExceededException(message);
                }

                var response = new PurchaseCornResponseDto
                {
                    IsSuccessful = true,
                    Message = message
                };

                _logger.LogInformation("Purchase successful for client: {ClientId}", request.ClientId);
                return Ok(response);
            }
            catch (RateLimitExceededException ex)
            {
                _logger.LogWarning("Rate limit exceeded for client: {ClientId}", request.ClientId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during purchase for client: {ClientId}", request.ClientId);
                throw;
            }
        }

        [HttpGet("history/{clientId}")]
        [ProducesResponseType(typeof(IEnumerable<Corn>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPurchaseHistory(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new OtherException("ClientId cannot be empty");
            }

            _logger.LogInformation("Retrieving purchase history for client: {ClientId}", clientId);

            try
            {
                var history = await _cornService.GetClientPurchaseHistoryAsync(clientId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving purchase history for client: {ClientId}", clientId);
                throw;
            }
        }

        [HttpGet("stats/{clientId}")]
        [ProducesResponseType(typeof(ClientHistoryResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetClientStats(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new OtherException("ClientId cannot be empty");
            }

            var purchases = await _cornService.GetClientPurchaseHistoryAsync(clientId);

            var stats = new ClientHistoryResponseDto
            {
                TotalPurchases = purchases.Count(),
                SuccessfulPurchases = purchases.Count(p => p.IsSuccessful),
                LastPurchaseTime = purchases.OrderByDescending(p => p.PurchaseTime).FirstOrDefault()?.PurchaseTime
            };

            _logger.LogInformation("Successful stats recovery for the client: {ClientId}", clientId);
            return Ok(stats);
        }
    }
}
