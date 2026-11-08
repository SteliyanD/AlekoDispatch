using AlekoDispatch.Data;
using AlekoDispatch.Data.Models;
using AlekoDispatch.ViewModels.WorkSites;
using Microsoft.EntityFrameworkCore;

namespace AlekoDispatch.Services
{
    public class WorkSiteService : IWorkSiteService
    {
        private readonly ApplicationDbContext _context;

        public WorkSiteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkSiteIndexViewModel>> GetAllAsync()
        {
            return await _context.WorkSites
                .AsNoTracking()
                .Select(ws => new WorkSiteIndexViewModel
                {
                    Id = ws.Id,
                    Name = ws.Name,
                    Location = ws.Location,
                    Status = ws.Status,
                    StartDate = ws.StartDate.ToString("dd.MM.yyyy"),
                    TargetCompletionDate = ws.TargetCompletionDate.HasValue
                        ? ws.TargetCompletionDate.Value.ToString("dd.MM.yyyy")
                        : null,
                    ActiveMachinesCount = ws.AssignedMachines.Count(m => m.IsOperational),
                    TotalLogsCount = ws.DailyWorkLogs.Count
                })
                .ToListAsync();
        }

        public async Task<WorkSiteDetailsViewModel?> GetDetailsByIdAsync(int id)
        {
            return await _context.WorkSites
                .AsNoTracking()
                .Where(ws => ws.Id == id)
                .Select(ws => new WorkSiteDetailsViewModel
                {
                    Id = ws.Id,
                    Name = ws.Name,
                    Location = ws.Location,
                    Description = ws.Description,
                    Status = ws.Status,
                    StartDate = ws.StartDate.ToString("dd.MM.yyyy"),
                    TargetCompletionDate = ws.TargetCompletionDate.HasValue
                        ? ws.TargetCompletionDate.Value.ToString("dd.MM.yyyy")
                        : null,
                    AssignedMachines = ws.AssignedMachines.Select(m => new WorkSiteAssignedMachineViewModel
                    {
                        Id = m.Id,
                        Model = m.Model,
                        InventoryNumber = m.InventoryNumber,
                        Type = m.Type,
                        CurrentEngineHours = m.CurrentEngineHours,
                        IsOperational = m.IsOperational
                    }).ToList(),
                    RecentLogs = ws.DailyWorkLogs
                        .OrderByDescending(l => l.LogDate)
                        .Take(5)
                        .Select(l => new WorkSiteRecentLogViewModel
                        {
                            Id = l.Id,
                            Date = l.LogDate.ToString("dd.MM.yyyy"),
                            WorkerName = l.OperatorName,
                            MachineModel = l.HeavyMachine.Model,
                            HoursWorked = l.HoursWorked,
                            Summary = l.Notes ?? string.Empty
                        }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<WorkSiteFormModel?> GetForEditByIdAsync(int id)
        {
            var entity = await _context.WorkSites.FindAsync(id);
            if (entity == null) return null;

            return new WorkSiteFormModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Location = entity.Location,
                Description = entity.Description,
                StartDate = entity.StartDate,
                TargetCompletionDate = entity.TargetCompletionDate,
                Status = entity.Status
            };
        }

        public async Task CreateAsync(WorkSiteFormModel model)
        {
            var entity = new WorkSite
            {
                Name = model.Name,
                Location = model.Location,
                Description = model.Description,
                StartDate = model.StartDate,
                TargetCompletionDate = model.TargetCompletionDate,
                Status = model.Status
            };

            await _context.WorkSites.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EditAsync(int id, WorkSiteFormModel model)
        {
            var entity = await _context.WorkSites.FindAsync(id);
            if (entity == null) return false;

            entity.Name = model.Name;
            entity.Location = model.Location;
            entity.Description = model.Description;
            entity.StartDate = model.StartDate;
            entity.TargetCompletionDate = model.TargetCompletionDate;
            entity.Status = model.Status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.WorkSites.FindAsync(id);
            if (entity == null) return false;

            _context.WorkSites.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
