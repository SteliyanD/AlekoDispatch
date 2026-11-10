using AlekoDispatch.Data;
using AlekoDispatch.Data.Enums;
using AlekoDispatch.Data.Models;
using AlekoDispatch.ViewModels.HeavyMachines;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AlekoDispatch.Services
{
    public class HeavyMachineService : IHeavyMachineService
    {
        private readonly ApplicationDbContext _context;

        public HeavyMachineService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HeavyMachineIndexViewModel>> GetAllAsync()
        {
            return await _context.HeavyMachines
                .AsNoTracking()
                .Select(m => new HeavyMachineIndexViewModel
                {
                    Id = m.Id,
                    Model = m.Model,
                    InventoryNumber = m.InventoryNumber,
                    Type = m.Type,
                    CurrentEngineHours = m.CurrentEngineHours,
                    IsOperational = m.IsOperational,
                    AssignedWorkSiteId = m.CurrentWorkSiteId,
                    AssignedWorkSiteName = m.CurrentWorkSite != null ? m.CurrentWorkSite.Name : null
                })
                .ToListAsync();
        }

        public async Task<HeavyMachineDetailsViewModel?> GetDetailsByIdAsync(int id)
        {
            return await _context.HeavyMachines
                .AsNoTracking()
                .Where(m => m.Id == id)
                .Select(m => new HeavyMachineDetailsViewModel
                {
                    Id = m.Id,
                    Model = m.Model,
                    InventoryNumber = m.InventoryNumber,
                    Type = m.Type,
                    CurrentEngineHours = m.CurrentEngineHours,
                    IsOperational = m.IsOperational,
                    CurrentWorkSiteId = m.CurrentWorkSiteId,
                    CurrentWorkSiteName = m.CurrentWorkSite != null ? m.CurrentWorkSite.Name : null,
                    TotalLogsCount = m.DailyWorkLogs.Count,
                    TotalHoursWorked = m.DailyWorkLogs.Sum(l => l.HoursWorked),
                    RecentLogs = m.DailyWorkLogs
                        .OrderByDescending(l => l.LogDate)
                        .Take(5)
                        .Select(l => new HeavyMachineLogSummaryViewModel
                        {
                            Date = l.LogDate.ToString("yyyy-MM-dd"),
                            OperatorName = l.OperatorName ?? "Unknown",
                            HoursWorked = l.HoursWorked,
                            FuelConsumedLiters = l.FuelConsumedLiters,
                            SiteName = l.WorkSite.Name
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<HeavyMachineFormModel> GetNewMachineFormModelAsync()
        {
            return new HeavyMachineFormModel
            {
                AvailableWorkSites = await GetWorkSitesSelectListAsync()
            };
        }

        public async Task CreateAsync(HeavyMachineFormModel model)
        {
            var machine = new HeavyMachine
            {
                Model = model.Model,
                InventoryNumber = model.InventoryNumber,
                Type = model.Type,
                CurrentEngineHours = model.CurrentEngineHours,
                IsOperational = model.IsOperational,
                CurrentWorkSiteId = model.CurrentWorkSiteId
            };

            await _context.HeavyMachines.AddAsync(machine);
            await _context.SaveChangesAsync();
        }

        public async Task<HeavyMachineFormModel?> GetForEditByIdAsync(int id)
        {
            var machine = await _context.HeavyMachines.FindAsync(id);
            if (machine == null)
            {
                return null;
            }

            return new HeavyMachineFormModel
            {
                Model = machine.Model,
                InventoryNumber = machine.InventoryNumber,
                Type = machine.Type,
                CurrentEngineHours = machine.CurrentEngineHours,
                IsOperational = machine.IsOperational,
                CurrentWorkSiteId = machine.CurrentWorkSiteId,
                AvailableWorkSites = await GetWorkSitesSelectListAsync()
            };
        }

        public async Task<bool> EditAsync(int id, HeavyMachineFormModel model)
        {
            var machine = await _context.HeavyMachines.FindAsync(id);
            if (machine == null)
            {
                return false;
            }

            machine.Model = model.Model;
            machine.InventoryNumber = model.InventoryNumber;
            machine.Type = model.Type;
            machine.CurrentEngineHours = model.CurrentEngineHours;
            machine.IsOperational = model.IsOperational;
            machine.CurrentWorkSiteId = model.CurrentWorkSiteId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var machine = await _context.HeavyMachines.FindAsync(id);
            if (machine == null)
            {
                return false;
            }

            _context.HeavyMachines.Remove(machine);
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
                    Text = s.Name
                })
                .ToListAsync();
        }
    }
}