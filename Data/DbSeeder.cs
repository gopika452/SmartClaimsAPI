using ClaimsPortal.Models;

namespace ClaimsPortal.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Check if database already has data
            if (context.Claims.Any())
            {
                return; // Database already seeded
            }

            var claims = new List<Claim>
            {
                new Claim
                {
                    PolicyNumber = "POL-2024-001",
                    ClaimantName = "John Smith",
                    DateOfLoss = new DateTime(2024, 10, 15),
                    ClaimAmount = 2500.00m,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow,
                    Remarks = "Vehicle collision claim"
                },
                new Claim
                {
                    PolicyNumber = "POL-2024-002",
                    ClaimantName = "Sarah Johnson",
                    DateOfLoss = new DateTime(2024, 11, 1),
                    ClaimAmount = 5000.00m,
                    Status = "Approved",
                    CreatedAt = DateTime.UtcNow,
                    Remarks = "Property damage - water leak"
                },
                new Claim
                {
                    PolicyNumber = "POL-2024-003",
                    ClaimantName = "Michael Brown",
                    DateOfLoss = new DateTime(2024, 11, 5),
                    ClaimAmount = 1200.50m,
                    Status = "Under Review",
                    CreatedAt = DateTime.UtcNow,
                    Remarks = "Medical expenses claim"
                }
            };

            await context.Claims.AddRangeAsync(claims);
            await context.SaveChangesAsync();
        }
    }
}

