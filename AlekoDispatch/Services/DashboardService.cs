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
            var totalWorkSites = await _context.WorkSites.CountAsync();
            var activeWorkSites = await _context.WorkSites
                .CountAsync(s => s.Status.ToString() == "Active");

            var totalMachines = await _context.HeavyMachines.CountAsync();
            var operationalMachines = await _context.HeavyMachines
                .CountAsync(m => m.IsOperational);

            var totalLoggedHours = await _context.DailyWorkLogs
                .SumAsync(l => (double?)l.HoursWorked) ?? 0;

            var totalFuelConsumed = await _context.DailyWorkLogs
                .SumAsync(l => (double?)l.FuelConsumedLiters) ?? 0;

            var totalDebrisHauled = await _context.DailyWorkLogs
                .SumAsync(l => (double?)l.DebrisHauledTons) ?? 0;

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
                TotalLoggedHours = totalLoggedHours,
                TotalFuelConsumedLiters = totalFuelConsumed,
                TotalDebrisHauledTons = totalDebrisHauled,
                RecentLogs = recentLogs
            };
        }
    }
}