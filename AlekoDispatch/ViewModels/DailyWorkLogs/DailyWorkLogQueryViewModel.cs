using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlekoDispatch.ViewModels.DailyWorkLogs
{
    public class DailyWorkLogQueryViewModel
    {
        public int? WorkSiteId { get; set; }

        public int? HeavyMachineId { get; set; }

        public string? SearchTerm { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int TotalLogs { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalLogs / PageSize);

        public IEnumerable<SelectListItem> WorkSites { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> Machines { get; set; } = new List<SelectListItem>();

        public IEnumerable<DailyWorkLogIndexViewModel> Logs { get; set; } = new List<DailyWorkLogIndexViewModel>();
    }
}