using AlekoDispatch.Common;
using AlekoDispatch.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace AlekoDispatch.Data.Models
{
    public class WorkSite
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.WorkSiteNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.WorkSiteLocationMaxLength)]
        public string Location { get; set; } = null!;

        [MaxLength(ValidationConstants.WorkSiteDescriptionMaxLength)]
        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? TargetCompletionDate { get; set; }

        [Required]
        public WorkSiteStatus Status { get; set; }

        public virtual ICollection<DailyWorkLog> DailyWorkLogs { get; set; } = new HashSet<DailyWorkLog>();

        public virtual ICollection<HeavyMachine> AssignedMachines { get; set; } = new HashSet<HeavyMachine>();
    }
}
