using AlekoDispatch.Data.Enums;

namespace AlekoDispatch.ViewModels.WorkSites
{
    public class WorkSiteIndexViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public WorkSiteStatus Status { get; set; }

        public string StartDate { get; set; } = string.Empty;

        public string? TargetCompletionDate { get; set; }

        public int ActiveMachinesCount { get; set; }

        public int TotalLogsCount { get; set; }
    }
}
