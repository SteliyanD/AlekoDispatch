using AlekoDispatch.ViewModels.DailyWorkLogs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlekoDispatch.Services
{
    public interface IDailyWorkLogService
    {
        Task<DailyWorkLogQueryViewModel> GetAllAsync(
            int? workSiteId = null,
            int? heavyMachineId = null,
            string? searchTerm = null,
            int currentPage = 1,
            int pageSize = 10);

        Task<DailyWorkLogFormModel> GetNewLogFormModelAsync(string? defaultOperatorName = null);

        Task CreateAsync(DailyWorkLogFormModel model);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<SelectListItem>> GetWorkSitesSelectListAsync();

        Task<IEnumerable<SelectListItem>> GetOperationalMachinesSelectListAsync();
    }
}