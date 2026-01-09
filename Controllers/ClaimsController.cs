using Microsoft.AspNetCore.Mvc;
using ClaimsPortal.Models;
using ClaimsPortal.Services;

namespace ClaimsPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimService _claimService;
        private readonly ILogger<ClaimsController> _logger;

        public ClaimsController(IClaimService claimService, ILogger<ClaimsController> logger)
        {
            _claimService = claimService;
            _logger = logger;
        }

        /// <summary>
        /// Get all claims
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Claim>>> GetAllClaims()
        {
            var claims = await _claimService.GetAllClaimsAsync();
            return Ok(claims);
        }

        /// <summary>
        /// Get a specific claim by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Claim>> GetClaim(int id)
        {
            var claim = await _claimService.GetClaimByIdAsync(id);
            
            if (claim == null)
            {
                return NotFound(new { message = $"Claim with ID {id} not found" });
            }

            return Ok(claim);
        }

        /// <summary>
        /// Create a new claim
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Claim>> CreateClaim([FromBody] Claim claim)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdClaim = await _claimService.CreateClaimAsync(claim);
            return CreatedAtAction(nameof(GetClaim), new { id = createdClaim.ClaimId }, createdClaim);
        }

        /// <summary>
        /// Update an existing claim
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Claim>> UpdateClaim(int id, [FromBody] Claim claim)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedClaim = await _claimService.UpdateClaimAsync(id, claim);
            
            if (updatedClaim == null)
            {
                return NotFound(new { message = $"Claim with ID {id} not found" });
            }

            return Ok(updatedClaim);
        }

        /// <summary>
        /// Delete a claim
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteClaim(int id)
        {
            var deleted = await _claimService.DeleteClaimAsync(id);
            
            if (!deleted)
            {
                return NotFound(new { message = $"Claim with ID {id} not found" });
            }

            return NoContent();
        }

        /// <summary>
        /// Import claims from an XML file
        /// </summary>
        [HttpPost("import/xml")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ImportClaimsFromXml(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded" });
            }

            if (!file.ContentType.Contains("xml") && !file.FileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { message = "Invalid file type. Please upload an XML file." });
            }

            try
            {
                using var stream = file.OpenReadStream();
                var importedCount = await _claimService.ImportClaimsFromXmlAsync(stream);
                
                return Ok(new 
                { 
                    message = "Claims imported successfully", 
                    importedCount = importedCount 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing XML file");
                return BadRequest(new { message = "Error processing XML file", error = ex.Message });
            }
        }
    }
}

