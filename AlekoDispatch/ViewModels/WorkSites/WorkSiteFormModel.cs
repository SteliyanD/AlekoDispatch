using AlekoDispatch.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace AlekoDispatch.ViewModels.WorkSites
{
    public class WorkSiteFormModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Site name is required.")]
        [StringLength(120, MinimumLength = 3, ErrorMessage = "Site name must be between 3 and 120 characters.")]
        [Display(Name = "Work Site Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Location must be between 2 and 150 characters.")]
        [Display(Name = "Location / Address")]
        public string Location { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        [Display(Name = "Project Scope / Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; } = DateTime.UtcNow.Date;

        [DataType(DataType.Date)]
        [Display(Name = "Target Completion Date")]
        public DateTime? TargetCompletionDate { get; set; }

        [Required]
        [Display(Name = "Project Status")]
        public WorkSiteStatus Status { get; set; } = WorkSiteStatus.Planned;
    }
}

