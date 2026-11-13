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

        public double TotalLoggedHours { get; set; }

        public double TotalFuelConsumedLiters { get; set; }

        public double TotalDebrisHauledTons { get; set; }

        public IEnumerable<DailyWorkLogIndexViewModel> RecentLogs { get; set; } = new List<DailyWorkLogIndexViewModel>();
    }
}