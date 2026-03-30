using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace oop_s2_2_mvc_76122.Models
{
    public enum Outcome
    {
        Pass,
        Fail
    }

    public class Inspection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Premises")]
        public int PremisesId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime InspectionDate { get; set; }

        [Required]
        [Range(0, 100)]
        public int Score { get; set; }

        [Required]
        public Outcome Outcome { get; set; }

        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;

        public virtual Premises? Premises { get; set; }
        public virtual ICollection<FollowUp> FollowUps { get; set; } = new List<FollowUp>();
    }
}