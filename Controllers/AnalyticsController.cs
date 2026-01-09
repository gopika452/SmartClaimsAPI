using ClaimsPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClaimsPortal.Controllers
{
    [ApiController]
    [Route("api/analytics")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IClaimService _claimService;

        public AnalyticsController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        /// <summary>
        /// Get count of claims grouped by status
        /// </summary>
        [HttpGet("status-summary")]
        public async Task<IActionResult> GetStatusSummary()
        {
            var result = await _claimService.GetClaimAnalyticsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get claims filtered by status
        /// </summary>
        [HttpGet("by-status/{status}")]
        public async Task<IActionResult> GetClaimsByStatus(string status)
        {
            var claims = await _claimService.GetClaimsByStatusAsync(status);
            return Ok(claims);
        }
    }
}
