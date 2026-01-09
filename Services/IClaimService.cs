using ClaimsPortal.Models;

namespace ClaimsPortal.Services
{
    public interface IClaimService
    {
        Task<IEnumerable<Claim>> GetAllClaimsAsync();
        Task<Claim?> GetClaimByIdAsync(int id);
        Task<Claim> CreateClaimAsync(Claim claim);
        Task<Claim?> UpdateClaimAsync(int id, Claim claim);
        Task<bool> DeleteClaimAsync(int id);
        Task<int> ImportClaimsFromXmlAsync(Stream xmlStream);

        // 🔹 ADD THESE TWO METHODS 👇
        Task<IEnumerable<Claim>> GetClaimsByStatusAsync(string status);
        Task<Dictionary<string, int>> GetClaimAnalyticsAsync();
    }
}


