using AlekoDispatch.Data.Enums;

namespace AlekoDispatch.ViewModels.HeavyMachines
{
    public class HeavyMachineDetailsViewModel
    {
        public int Id { get; set; }

        public string Model { get; set; } = null!;

        public string InventoryNumber { get; set; } = null!;

        public MachineType Type { get; set; }

        public double CurrentEngineHours { get; set; }

        public bool IsOperational { get; set; }

        public string? CurrentWorkSiteName { get; set; }

        public int? CurrentWorkSiteId { get; set; }

        public int TotalLogsCount { get; set; }

        public double TotalHoursWorked { get; set; }

        public List<HeavyMachineLogSummaryViewModel> RecentLogs { get; set; } = new();
    }

    public class HeavyMachineLogSummaryViewModel
    {
        public string Date { get; set; } = null!;

        public string OperatorName { get; set; } = null!;

        public double HoursWorked { get; set; }

        public double? FuelConsumedLiters { get; set; }

        public string SiteName { get; set; } = null!;
    }
}
