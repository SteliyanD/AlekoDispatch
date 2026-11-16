
using AlekoDispatch.Common;
using AlekoDispatch.Data.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AlekoDispatch.ViewModels.HeavyMachines
{
    public class HeavyMachineFormModel
    {
        [Required]
        [StringLength(
            ValidationConstants.MachineModelMaxLength,
            MinimumLength = ValidationConstants.MachineModelMinLength,
            ErrorMessage = "Model must be between {2} and {1} characters.")]
        public string Model { get; set; } = null!;

        [Required]
        [StringLength(
            ValidationConstants.MachineRegNumberMaxLength,
            MinimumLength = ValidationConstants.MachineRegNumberMinLength,
            ErrorMessage = "Inventory number must be between {2} and {1} characters.")]
        [Display(Name = "Inventory / Registration Number")]
        public string InventoryNumber { get; set; } = null!;

        [Required]
        [Display(Name = "Machine Type")]
        public MachineType Type { get; set; }

        [Range(
            ValidationConstants.MachineHoursMinValue,
            ValidationConstants.MachineHoursMaxValue)]
        [Display(Name = "Initial / Current Engine Hours")]
        public double CurrentEngineHours { get; set; }

        [Display(Name = "Operational Readiness")]
        public bool IsOperational { get; set; } = true;

        [Display(Name = "Assigned Work Site")]
        public int? CurrentWorkSiteId { get; set; }

        public IEnumerable<SelectListItem> AvailableWorkSites { get; set; }
            = new List<SelectListItem>();
    }
}
