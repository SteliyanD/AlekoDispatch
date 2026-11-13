using AlekoDispatch.ViewModels.Home;

namespace AlekoDispatch.Services
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardStatsAsync();
    }
}