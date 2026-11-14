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

        public async Task<DailyWorkLogQueryViewModel> GetAllAsync(
            int? workSiteId = null,
            int? heavyMachineId = null,
            string? searchTerm = null,
            int currentPage = 1,
            int pageSize = 10)
        {
            if (currentPage < 1)
            {
                currentPage = 1;
            }

            var query = _context.DailyWorkLogs
                .AsNoTracking()
                .AsQueryable();

            if (workSiteId.HasValue)
            {
                query = query.Where(l => l.WorkSiteId == workSiteId.Value);
            }

            if (heavyMachineId.HasValue)
            {
                query = query.Where(l => l.HeavyMachineId == heavyMachineId.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalizedSearch = searchTerm.Trim().ToLower();
                query = query.Where(l =>
                    l.OperatorName.ToLower().Contains(normalizedSearch) ||
                    (l.Notes != null && l.Notes.ToLower().Contains(normalizedSearch)));
            }

            var totalLogs = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogs / pageSize);

            if (totalPages > 0 && currentPage > totalPages)
            {
                currentPage = totalPages;
            }

            var logs = await query
                .OrderByDescending(l => l.LogDate)
                .ThenByDescending(l => l.Id)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
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

            return new DailyWorkLogQueryViewModel
            {
                WorkSiteId = workSiteId,
                HeavyMachineId = heavyMachineId,
                SearchTerm = searchTerm,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalLogs = totalLogs,
                WorkSites = await GetWorkSitesSelectListAsync(),
                Machines = await GetAllMachinesSelectListAsync(),
                Logs = logs
            };
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

        public async Task<IEnumerable<SelectListItem>> GetAllMachinesSelectListAsync()
        {
            return await _context.HeavyMachines
                .AsNoTracking()
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