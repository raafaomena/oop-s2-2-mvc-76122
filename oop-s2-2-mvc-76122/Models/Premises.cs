using System.ComponentModel.DataAnnotations;

namespace oop_s2_2_mvc_76122.Models
{
    public enum RiskRating
    {
        Low,
        Medium,
        High
    }

    public class Premises
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Town { get; set; } = string.Empty;

        [Required]
        public RiskRating RiskRating { get; set; }

        public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
    }
}