using AlekoDispatch.Data.Enums;

namespace AlekoDispatch.ViewModels.WorkSites
{
    public class WorkSiteQueryViewModel
    {
        public string? SearchTerm { get; set; }

        public WorkSiteStatus? Status { get; set; }

        public string SortOrder { get; set; } = "date_desc";

        public IEnumerable<WorkSiteIndexViewModel> WorkSites { get; set; } = new List<WorkSiteIndexViewModel>();
    }
}