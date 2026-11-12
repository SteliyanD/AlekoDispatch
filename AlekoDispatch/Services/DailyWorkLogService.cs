using AlekoDispatch.Data;
using AlekoDispatch.Data.Models;
using AlekoDispatch.ViewModels.DailyWorkLogs;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AlekoDispatch.Services
{
    public class DailyWorkLogService : IDailyWorkLogService
    {
        private readonly ApplicationDbContext _context;

        public DailyWorkLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DailyWorkLogIndexViewModel>> GetAllAsync()
        {
            return await _context.DailyWorkLogs
                .AsNoTracking()
                .OrderByDescending(l => l.LogDate)
                .ThenByDescending(l => l.Id)
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
        }

        public async Task<DailyWorkLogFormModel> GetNewLogFormModelAsync(string? defaultOperatorName = null)
        {
            return new DailyWorkLogFormModel
            {
                LogDate = DateTime.Today,
                OperatorName = defaultOperatorName ?? string.Empty,
                AvailableWorkSites = await GetWorkSitesSelectListAsync(),
                AvailableMachines = await GetOperationalMachinesSelectListAsync()
            };
        }

        public async Task CreateAsync(DailyWorkLogFormModel model)
        {
            var log = new DailyWorkLog
            {
                LogDate = model.LogDate,
                WorkSiteId = model.WorkSiteId,
                HeavyMachineId = model.HeavyMachineId,
                HoursWorked = model.HoursWorked,
                FuelConsumedLiters = model.FuelConsumedLiters,
                DebrisHauledTons = model.DebrisHauledTons,
                OperatorName = model.OperatorName,
                Notes = model.Notes
            };

            await _context.DailyWorkLogs.AddAsync(log);

            var machine = await _context.HeavyMachines.FindAsync(model.HeavyMachineId);
            if (machine != null)
            {
                machine.CurrentEngineHours += model.HoursWorked;
                machine.CurrentWorkSiteId = model.WorkSiteId;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var log = await _context.DailyWorkLogs.FindAsync(id);
            if (log == null)
            {
                return false;
            }

            var machine = await _context.HeavyMachines.FindAsync(log.HeavyMachineId);
            if (machine != null)
            {
                machine.CurrentEngineHours = Math.Max(0, machine.CurrentEngineHours - log.HoursWorked);
            }

            _context.DailyWorkLogs.Remove(log);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SelectListItem>> GetWorkSitesSelectListAsync()
        {
            return await _context.WorkSites
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.Name} ({s.Location})"
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetOperationalMachinesSelectListAsync()
        {
            return await _context.HeavyMachines
                .AsNoTracking()
                .Where(m => m.IsOperational)
                .OrderBy(m => m.Model)
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = $"{m.Model} [{m.InventoryNumber}]"
                })
                .ToListAsync();
        }
    }
}