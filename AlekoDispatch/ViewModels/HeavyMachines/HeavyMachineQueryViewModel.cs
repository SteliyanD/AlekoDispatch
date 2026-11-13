using AlekoDispatch.Data.Enums;

namespace AlekoDispatch.ViewModels.HeavyMachines
{
    public class HeavyMachineQueryViewModel
    {
        public string? SearchTerm { get; set; }

        public MachineType? Category { get; set; }

        public bool? IsOperational { get; set; }

        public IEnumerable<HeavyMachineIndexViewModel> Machines { get; set; } = new List<HeavyMachineIndexViewModel>();
    }
}