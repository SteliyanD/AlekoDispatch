namespace AlekoDispatch.ViewModels.DailyWorkLogs
{
    public class DailyWorkLogIndexViewModel
    {
        public int Id { get; set; }

        public string LogDate { get; set; } = null!;

        public int WorkSiteId { get; set; }

        public string WorkSiteName { get; set; } = null!;

        public int HeavyMachineId { get; set; }

        public string MachineName { get; set; } = null!;

        public string MachineInventoryNumber { get; set; } = null!;

        public string OperatorName { get; set; } = null!;

        public double HoursWorked { get; set; }

        public double FuelConsumedLiters { get; set; }

        public double DebrisHauledTons { get; set; }

        public string? Notes { get; set; }
    }
}