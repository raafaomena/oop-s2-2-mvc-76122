using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace oop_s2_2_mvc_76122.Models
{
    public enum Status
    {
        Open,
        Closed
    }

    public class FollowUp
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Inspection")]
        public int InspectionId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Required]
        public Status Status { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ClosedDate { get; set; }

        public virtual Inspection? Inspection { get; set; }
    }
}