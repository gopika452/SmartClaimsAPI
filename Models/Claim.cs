using System.ComponentModel.DataAnnotations;

namespace ClaimsPortal.Models
{
    public class Claim
    {
        [Key]
        public int ClaimId { get; set; }

        [Required]
        [MaxLength(50)]
        public string PolicyNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ClaimantName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfLoss { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal ClaimAmount { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? Remarks { get; set; }
    }
}

