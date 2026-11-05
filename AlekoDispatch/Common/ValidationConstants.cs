namespace AlekoDispatch.Common
{
    public class ValidationConstants
    {
        // WorkSite
        public const int WorkSiteNameMinLength = 3;
        public const int WorkSiteNameMaxLength = 100;

        public const int WorkSiteLocationMinLength = 3;
        public const int WorkSiteLocationMaxLength = 150;

        public const int WorkSiteDescriptionMaxLength = 500;

        // HeavyMachine
        public const int MachineModelMinLength = 2;
        public const int MachineModelMaxLength = 60;

        public const int MachineRegNumberMinLength = 4;
        public const int MachineRegNumberMaxLength = 20;

        public const double MachineHoursMinValue = 0.0;
        public const double MachineHoursMaxValue = 100000.0;

        // DailyWorkLog
        public const double LogHoursWorkedMinValue = 0.5;
        public const double LogHoursWorkedMaxValue = 24.0;

        public const double LogFuelConsumedMinValue = 0.0;
        public const double LogFuelConsumedMaxValue = 5000.0;

        public const double LogDebrisHauledMinValue = 0.0;
        public const double LogDebrisHauledMaxValue = 2000.0;

        public const int LogOperatorNameMinLength = 2;
        public const int LogOperatorNameMaxLength = 80;

        public const int LogNotesMaxLength = 1000;
    }
}
