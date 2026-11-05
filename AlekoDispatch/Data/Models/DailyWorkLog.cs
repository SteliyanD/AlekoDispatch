using AlekoDispatch.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlekoDispatch.Data.Models
{
    public class DailyWorkLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime LogDate { get; set; }

        [Required]
        public int WorkSiteId { get; set; }

        [ForeignKey(nameof(WorkSiteId))]
        public virtual WorkSite WorkSite { get; set; } = null!;

        [Required]
        public int HeavyMachineId { get; set; }

        [ForeignKey(nameof(HeavyMachineId))]
        public virtual HeavyMachine HeavyMachine { get; set; } = null!;

        [Range(ValidationConstants.LogHoursWorkedMinValue, ValidationConstants.LogHoursWorkedMaxValue)]
        public double HoursWorked { get; set; }

        [Range(ValidationConstants.LogFuelConsumedMinValue, ValidationConstants.LogFuelConsumedMaxValue)]
        public double FuelConsumedLiters { get; set; }

        [Range(ValidationConstants.LogDebrisHauledMinValue, ValidationConstants.LogDebrisHauledMaxValue)]
        public double DebrisHauledTons { get; set; }

        [Required]
        [MaxLength(ValidationConstants.LogOperatorNameMaxLength)]
        public string OperatorName { get; set; } = null!;

        [MaxLength(ValidationConstants.LogNotesMaxLength)]
        public string? Notes { get; set; }
    }
}
