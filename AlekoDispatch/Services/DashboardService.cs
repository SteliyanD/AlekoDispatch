using AlekoDispatch.Data;
using AlekoDispatch.ViewModels.DailyWorkLogs;
using AlekoDispatch.ViewModels.Home;
using Microsoft.EntityFrameworkCore;

namespace AlekoDispatch.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> GetDashboardStatsAsync()
        {
            var today = DateTime.Today;

            var totalWorkSites = await _context.WorkSites.CountAsync();
            var activeWorkSites = await _context.WorkSites
                .CountAsync(s => s.Status.ToString() == "Active");

            var totalMachines = await _context.HeavyMachines.CountAsync();
            var operationalMachines = await _context.HeavyMachines
                .CountAsync(m => m.IsOperational);

            var totalEmployees = await _context.Users.CountAsync();

            var logsSubmittedToday = await _context.DailyWorkLogs
                .CountAsync(l => l.LogDate.Date == today);

            var fuelConsumedToday = await _context.DailyWorkLogs
                .Where(l => l.LogDate.Date == today)
                .SumAsync(l => (double?)l.FuelConsumedLiters) ?? 0;

            var recentLogs = await _context.DailyWorkLogs
                .AsNoTracking()
                .OrderByDescending(l => l.LogDate)
                .ThenByDescending(l => l.Id)
                .Take(5)
                .Select(l => new DailyWorkLogIndexViewModel
                {
                    Id = l.Id,
                    LogDate = l.LogDate.ToString("yyyy-MM-dd"),
                    WorkSiteId = l.WorkSiteId,
                    WorkSiteName = l.WorkSite.Name,
                    HeavyMachineId = l.HeavyMachineId,
                    MachineName = l.HeavyMachine.Model,
                    MachineInventoryNumber = l.HeavyMachine.InventoryNumber,
                    OperatorName = l.OperatorName,
                    HoursWorked = l.HoursWorked,
                    FuelConsumedLiters = l.FuelConsumedLiters,
                    DebrisHauledTons = l.DebrisHauledTons,
                    Notes = l.Notes
                })
                .ToListAsync();

            return new DashboardViewModel
            {
                TotalWorkSites = totalWorkSites,
                ActiveWorkSites = activeWorkSites,
                TotalMachines = totalMachines,
                OperationalMachines = operationalMachines,
                MaintenanceMachines = totalMachines - operationalMachines,
                TotalEmployees = totalEmployees,
                LogsSubmittedToday = logsSubmittedToday,
                FuelConsumedTodayLiters = fuelConsumedToday,
                RecentLogs = recentLogs
            };
        }
    }
}