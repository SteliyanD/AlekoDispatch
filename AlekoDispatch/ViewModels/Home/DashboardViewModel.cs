using AlekoDispatch.ViewModels.DailyWorkLogs;

namespace AlekoDispatch.ViewModels.Home
{
    public class DashboardViewModel
    {
        public int TotalWorkSites { get; set; }

        public int ActiveWorkSites { get; set; }

        public int TotalMachines { get; set; }

        public int OperationalMachines { get; set; }

        public int MaintenanceMachines { get; set; }

        public int TotalEmployees { get; set; }

        public int LogsSubmittedToday { get; set; }

        public double FuelConsumedTodayLiters { get; set; }

        public IEnumerable<DailyWorkLogIndexViewModel> RecentLogs { get; set; } = new List<DailyWorkLogIndexViewModel>();
    }
}