using AlekoDispatch.Data.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AlekoDispatch.ViewModels.HeavyMachines
{
    public class HeavyMachineFormModel
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Model { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Inventory / Registration Number")]
        public string InventoryNumber { get; set; } = null!;

        [Required]
        [Display(Name = "Machine Type")]
        public MachineType Type { get; set; }

        [Range(0, 100000.0)]
        [Display(Name = "Initial / Current Engine Hours")]
        public double CurrentEngineHours { get; set; }

        [Display(Name = "Operational Readiness")]
        public bool IsOperational { get; set; } = true;

        [Display(Name = "Assigned Work Site")]
        public int? CurrentWorkSiteId { get; set; }

        public IEnumerable<SelectListItem> AvailableWorkSites { get; set; } = new List<SelectListItem>();
    }
}
