using System.ComponentModel.DataAnnotations;
using AlekoDispatch.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlekoDispatch.ViewModels.DailyWorkLogs
{
    public class DailyWorkLogFormModel
    {
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Shift / Log Date")]
        public DateTime LogDate { get; set; } = DateTime.Today;

        [Required]
        [Display(Name = "Work Site")]
        public int WorkSiteId { get; set; }

        [Required]
        [Display(Name = "Heavy Machine")]
        public int HeavyMachineId { get; set; }

        [Required]
        [Range(ValidationConstants.LogHoursWorkedMinValue, ValidationConstants.LogHoursWorkedMaxValue)]
        [Display(Name = "Hours Worked")]
        public double HoursWorked { get; set; }

        [Required]
        [Range(ValidationConstants.LogFuelConsumedMinValue, ValidationConstants.LogFuelConsumedMaxValue)]
        [Display(Name = "Fuel Consumed (Liters)")]
        public double FuelConsumedLiters { get; set; }

        [Required]
        [Range(ValidationConstants.LogDebrisHauledMinValue, ValidationConstants.LogDebrisHauledMaxValue)]
        [Display(Name = "Debris Hauled (Tons)")]
        public double DebrisHauledTons { get; set; }

        [Required]
        [MaxLength(ValidationConstants.LogOperatorNameMaxLength)]
        [Display(Name = "Operator Name")]
        public string OperatorName { get; set; } = null!;

        [MaxLength(ValidationConstants.LogNotesMaxLength)]
        [Display(Name = "Shift Notes / Observations")]
        public string? Notes { get; set; }

        public IEnumerable<SelectListItem> AvailableWorkSites { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> AvailableMachines { get; set; } = new List<SelectListItem>();
    }
}