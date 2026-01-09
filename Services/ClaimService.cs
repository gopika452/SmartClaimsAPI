using System.Xml.Linq;
using ClaimsPortal.Data;
using ClaimsPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaimsPortal.Services
{
    public class ClaimService : IClaimService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ClaimService> _logger;

        public ClaimService(AppDbContext context, ILogger<ClaimService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Claim>> GetAllClaimsAsync()
        {
            _logger.LogInformation("Retrieving all claims");
            return await _context.Claims
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Claim?> GetClaimByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving claim with ID: {ClaimId}", id);
            return await _context.Claims.FindAsync(id);
        }

        public async Task<Claim> CreateClaimAsync(Claim claim)
        {
            _logger.LogInformation("Creating new claim for policy: {PolicyNumber}", claim.PolicyNumber);

            claim.CreatedAt = DateTime.UtcNow;
            if (string.IsNullOrWhiteSpace(claim.Status))
            {
                claim.Status = "Pending";
            }

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Claim created with ID: {ClaimId}", claim.ClaimId);
            return claim;
        }

        public async Task<Claim?> UpdateClaimAsync(int id, Claim claim)
        {
            _logger.LogInformation("Updating claim with ID: {ClaimId}", id);

            var existingClaim = await _context.Claims.FindAsync(id);
            if (existingClaim == null)
            {
                _logger.LogWarning("Claim with ID {ClaimId} not found", id);
                return null;
            }

            existingClaim.PolicyNumber = claim.PolicyNumber;
            existingClaim.ClaimantName = claim.ClaimantName;
            existingClaim.DateOfLoss = claim.DateOfLoss;
            existingClaim.ClaimAmount = claim.ClaimAmount;
            existingClaim.Status = claim.Status;
            existingClaim.Remarks = claim.Remarks;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Claim with ID {ClaimId} updated successfully", id);
            return existingClaim;
        }

        public async Task<bool> DeleteClaimAsync(int id)
        {
            _logger.LogInformation("Deleting claim with ID: {ClaimId}", id);

            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                _logger.LogWarning("Claim with ID {ClaimId} not found", id);
                return false;
            }

            _context.Claims.Remove(claim);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Claim with ID {ClaimId} deleted successfully", id);
            return true;
        }

        public async Task<int> ImportClaimsFromXmlAsync(Stream xmlStream)
        {
            _logger.LogInformation("Starting XML import");

            try
            {
                var document = await XDocument.LoadAsync(xmlStream, LoadOptions.None, CancellationToken.None);
                var claimElements = document.Descendants("Claim");

                var claims = new List<Claim>();

                foreach (var element in claimElements)
                {
                    var policyNumber = element.Element("PolicyNumber")?.Value;
                    var claimantName = element.Element("ClaimantName")?.Value;
                    var claimAmountStr = element.Element("ClaimAmount")?.Value;
                    var dateOfLossStr = element.Element("DateOfLoss")?.Value;
                    var status = element.Element("Status")?.Value ?? "Pending";
                    var remarks = element.Element("Remarks")?.Value;

                    if (string.IsNullOrWhiteSpace(policyNumber) ||
                        string.IsNullOrWhiteSpace(claimantName) ||
                        string.IsNullOrWhiteSpace(claimAmountStr) ||
                        string.IsNullOrWhiteSpace(dateOfLossStr))
                    {
                        _logger.LogWarning("Skipping invalid claim entry in XML");
                        continue;
                    }

                    if (!decimal.TryParse(claimAmountStr, out var claimAmount))
                    {
                        _logger.LogWarning("Invalid claim amount: {ClaimAmount}", claimAmountStr);
                        continue;
                    }

                    if (!DateTime.TryParse(dateOfLossStr, out var dateOfLoss))
                    {
                        _logger.LogWarning("Invalid date of loss: {DateOfLoss}", dateOfLossStr);
                        continue;
                    }

                    claims.Add(new Claim
                    {
                        PolicyNumber = policyNumber,
                        ClaimantName = claimantName,
                        ClaimAmount = claimAmount,
                        DateOfLoss = dateOfLoss,
                        Status = status,
                        Remarks = remarks,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                if (claims.Any())
                {
                    await _context.Claims.AddRangeAsync(claims);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Successfully imported {Count} claims from XML", claims.Count);
                }

                return claims.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing claims from XML");
                throw;
            }
        }

        // 🔹 NEW METHOD 1: FILTER CLAIMS BY STATUS (FOR GRAPHS)
        public async Task<IEnumerable<Claim>> GetClaimsByStatusAsync(string status)
        {
            _logger.LogInformation("Retrieving claims with status: {Status}", status);

            return await _context.Claims
                .Where(c => c.Status == status)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        // 🔹 NEW METHOD 2: CLAIM ANALYTICS (FOR BAR / LINE CHART)
        public async Task<Dictionary<string, int>> GetClaimAnalyticsAsync()
        {
            _logger.LogInformation("Generating claim analytics");

            return await _context.Claims
                .GroupBy(c => c.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToDictionaryAsync(x => x.Status, x => x.Count);
        }
    }
}

