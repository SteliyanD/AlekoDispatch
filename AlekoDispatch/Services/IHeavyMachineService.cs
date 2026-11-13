using AlekoDispatch.Data.Enums;
using AlekoDispatch.ViewModels.HeavyMachines;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlekoDispatch.Services
{
    public interface IHeavyMachineService
    {
        Task<HeavyMachineQueryViewModel> GetAllAsync(
            string? searchTerm = null,
            MachineType? category = null,
            bool? isOperational = null);

        Task<HeavyMachineDetailsViewModel?> GetDetailsByIdAsync(int id);

        Task<HeavyMachineFormModel> GetNewMachineFormModelAsync();

        Task CreateAsync(HeavyMachineFormModel model);

        Task<HeavyMachineFormModel?> GetForEditByIdAsync(int id);

        Task<bool> EditAsync(int id, HeavyMachineFormModel model);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<SelectListItem>> GetWorkSitesSelectListAsync();
    }
}
