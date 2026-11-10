using AlekoDispatch.Data.Enums;

namespace AlekoDispatch.ViewModels.HeavyMachines
{
    public class HeavyMachineIndexViewModel
    {
        public int Id { get; set; }

        public string Model { get; set; } = null!;

        public string InventoryNumber { get; set; } = null!;

        public MachineType Type { get; set; }

        public double CurrentEngineHours { get; set; }

        public bool IsOperational { get; set; }

        public string? AssignedWorkSiteName { get; set; }

        public int? AssignedWorkSiteId { get; set; }
    }
}
