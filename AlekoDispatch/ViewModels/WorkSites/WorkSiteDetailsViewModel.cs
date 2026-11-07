using AlekoDispatch.Data.Enums;

namespace AlekoDispatch.ViewModels.WorkSites
{
    public class WorkSiteDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string? Description { get; set; }

        public WorkSiteStatus Status { get; set; }

        public string StartDate { get; set; } = string.Empty;

        public string? TargetCompletionDate { get; set; }

        public List<WorkSiteAssignedMachineViewModel> AssignedMachines { get; set; } = new();

        public List<WorkSiteRecentLogViewModel> RecentLogs { get; set; } = new();
    }

    public class WorkSiteAssignedMachineViewModel
    {
        public int Id { get; set; }

        public string Model { get; set; } = string.Empty;

        public string InventoryNumber { get; set; } = string.Empty;

        public MachineType Type { get; set; }

        public double CurrentEngineHours { get; set; }

        public bool IsOperational { get; set; }
    }

    public class WorkSiteRecentLogViewModel
    {
        public int Id { get; set; }

        public string Date { get; set; } = string.Empty;

        public string WorkerName { get; set; } = string.Empty;

        public string MachineModel { get; set; } = string.Empty;

        public double HoursWorked { get; set; }

        public string Summary { get; set; } = string.Empty;
    }
}
