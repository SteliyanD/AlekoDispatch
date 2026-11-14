using AlekoDispatch.Data.Enums;
using AlekoDispatch.ViewModels.WorkSites;

namespace AlekoDispatch.Services
{
    public interface IWorkSiteService
    {
        Task<WorkSiteQueryViewModel> GetAllAsync(
            string? searchTerm = null,
            WorkSiteStatus? status = null,
            string sortOrder = "date_desc");
        Task<WorkSiteDetailsViewModel?> GetDetailsByIdAsync(int id);
        Task<WorkSiteFormModel?> GetForEditByIdAsync(int id);
        Task CreateAsync(WorkSiteFormModel model);
        Task<bool> EditAsync(int id, WorkSiteFormModel model);
        Task<bool> DeleteAsync(int id);
    }
}
