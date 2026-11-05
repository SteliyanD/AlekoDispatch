using AlekoDispatch.Common;
using AlekoDispatch.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlekoDispatch.Data.Models
{
    public class HeavyMachine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.MachineModelMaxLength)]
        public string Model { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.MachineRegNumberMaxLength)]
        public string InventoryNumber { get; set; } = null!;

        [Required]
        public MachineType Type { get; set; }

        [Range(ValidationConstants.MachineHoursMinValue, ValidationConstants.MachineHoursMaxValue)]
        public double CurrentEngineHours { get; set; }

        public bool IsOperational { get; set; } = true;

        public int? CurrentWorkSiteId { get; set; }

        [ForeignKey(nameof(CurrentWorkSiteId))]
        public virtual WorkSite? CurrentWorkSite { get; set; }

        public virtual ICollection<DailyWorkLog> DailyWorkLogs { get; set; } = new HashSet<DailyWorkLog>();
    }
}
