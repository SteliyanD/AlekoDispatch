using AlekoDispatch.ViewModels.WorkSites;

namespace AlekoDispatch.Services
{
    public interface IWorkSiteService
    {
        Task<IEnumerable<WorkSiteIndexViewModel>> GetAllAsync();
        Task<WorkSiteDetailsViewModel?> GetDetailsByIdAsync(int id);
        Task<WorkSiteFormModel?> GetForEditByIdAsync(int id);
        Task CreateAsync(WorkSiteFormModel model);
        Task<bool> EditAsync(int id, WorkSiteFormModel model);
        Task<bool> DeleteAsync(int id);
    }
}
