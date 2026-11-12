using AlekoDispatch.ViewModels.DailyWorkLogs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlekoDispatch.Services
{
    public interface IDailyWorkLogService
    {
        Task<IEnumerable<DailyWorkLogIndexViewModel>> GetAllAsync();

        Task<DailyWorkLogFormModel> GetNewLogFormModelAsync(string? defaultOperatorName = null);

        Task CreateAsync(DailyWorkLogFormModel model);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<SelectListItem>> GetWorkSitesSelectListAsync();

        Task<IEnumerable<SelectListItem>> GetOperationalMachinesSelectListAsync();
    }
}