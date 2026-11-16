using AlekoDispatch.Data.Enums;
using AlekoDispatch.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AlekoDispatch.Data
{
    public class DbInitializer
    {
        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();

            // Roles setup
            string[] roleNames = { "Administrator", "Employee" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Default administrator account
            string adminEmail = "admin@aleko.bg";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var createAdminResult = await userManager.CreateAsync(adminUser, "Admin123!");
                if (createAdminResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrator");
                }
            }

            // Default employee account for testing non-admin role access
            string employeeEmail = "employee@aleko.bg";
            if (await userManager.FindByEmailAsync(employeeEmail) == null)
            {
                var employeeUser = new IdentityUser
                {
                    UserName = employeeEmail,
                    Email = employeeEmail,
                    EmailConfirmed = true
                };

                var createEmployeeResult = await userManager.CreateAsync(employeeUser, "Employee123!");
                if (createEmployeeResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(employeeUser, "Employee");
                }
            }

            // WorkSites seeding with static dates respecting StartDate <= TargetCompletionDate / CompletedDate
            if (!await context.WorkSites.AnyAsync())
            {
                var workSites = new List<WorkSite>
                {
                    new WorkSite
                    {
                        Name = "Industrial Warehouse Demolition - West Zone",
                        Location = "Pleven, Industrial District",
                        Description = "Complete teardown of reinforced concrete structure, crushing work, and debris hauling.",
                        StartDate = new DateTime(2026, 10, 15),
                        TargetCompletionDate = new DateTime(2026, 12, 10),
                        Status = WorkSiteStatus.Active
                    },
                    new WorkSite
                    {
                        Name = "Commercial Logistics Base Excavation",
                        Location = "Pleven, Evropa Blvd",
                        Description = "Foundation pit excavation, bulk earthmoving, and site clearing.",
                        StartDate = new DateTime(2026, 11, 1),
                        TargetCompletionDate = new DateTime(2026, 11, 25),
                        Status = WorkSiteStatus.Active
                    },
                    new WorkSite
                    {
                        Name = "Reinforced Concrete Slab Dismantling",
                        Location = "Yasen, Pleven District",
                        Description = "Hydraulic hammer demolition of industrial concrete apron and rubble loading.",
                        StartDate = new DateTime(2026, 11, 20),
                        TargetCompletionDate = new DateTime(2026, 12, 5),
                        Status = WorkSiteStatus.Planned
                    },
                    new WorkSite
                    {
                        Name = "Old Grain Silo Foundation Clearing",
                        Location = "Dolna Mitropoliya, Pleven District",
                        Description = "Foundation removal, site grading, and concrete rubble transport.",
                        StartDate = new DateTime(2026, 9, 1),
                        TargetCompletionDate = new DateTime(2026, 10, 10),
                        CompletedDate = new DateTime(2026, 10, 12),
                        Status = WorkSiteStatus.Completed
                    }
                };

                await context.WorkSites.AddRangeAsync(workSites);
                await context.SaveChangesAsync();
            }

            // HeavyMachines seeding (including an out-of-service unit to demonstrate operational filtering)
            if (!await context.HeavyMachines.AnyAsync())
            {
                var activeSites = await context.WorkSites
                    .Where(s => s.Status == WorkSiteStatus.Active)
                    .OrderBy(s => s.Id)
                    .ToListAsync();

                var firstActiveSite = activeSites.FirstOrDefault();
                var secondActiveSite = activeSites.Skip(1).FirstOrDefault();

                var machines = new List<HeavyMachine>
                {
                    new HeavyMachine
                    {
                        Model = "CAT 320 GC",
                        InventoryNumber = "EH-104",
                        Type = MachineType.Excavator,
                        CurrentEngineHours = 3450.5,
                        IsOperational = true,
                        CurrentWorkSiteId = firstActiveSite?.Id
                    },
                    new HeavyMachine
                    {
                        Model = "Komatsu PC210LC",
                        InventoryNumber = "EH-105",
                        Type = MachineType.Excavator,
                        CurrentEngineHours = 5120.0,
                        IsOperational = true,
                        CurrentWorkSiteId = firstActiveSite?.Id
                    },
                    new HeavyMachine
                    {
                        Model = "JCB 4CX",
                        InventoryNumber = "EH-201",
                        Type = MachineType.WheelLoader,
                        CurrentEngineHours = 2100.8,
                        IsOperational = true,
                        CurrentWorkSiteId = secondActiveSite?.Id
                    },
                    new HeavyMachine
                    {
                        Model = "Mercedes-Benz Arocs 4145 (4-Axle)",
                        InventoryNumber = "EH-302",
                        Type = MachineType.DumpTruck,
                        CurrentEngineHours = 6800.2,
                        IsOperational = true,
                        CurrentWorkSiteId = firstActiveSite?.Id
                    },
                    new HeavyMachine
                    {
                        Model = "Volvo EC250E (Hydraulic Maintenance)",
                        InventoryNumber = "EH-109",
                        Type = MachineType.Excavator,
                        CurrentEngineHours = 7410.0,
                        IsOperational = false,
                        CurrentWorkSiteId = null
                    }
                };

                await context.HeavyMachines.AddRangeAsync(machines);
                await context.SaveChangesAsync();
            }

            // DailyWorkLogs seeding (strictly linked to active work sites, operational machines, and system user emails)
            if (!await context.DailyWorkLogs.AnyAsync())
            {
                var activeSite = await context.WorkSites
                    .Where(s => s.Status == WorkSiteStatus.Active)
                    .OrderBy(s => s.Id)
                    .FirstOrDefaultAsync();

                var excavator = await context.HeavyMachines
                    .FirstOrDefaultAsync(m => m.InventoryNumber == "EH-104" && m.IsOperational);

                var dumpTruck = await context.HeavyMachines
                    .FirstOrDefaultAsync(m => m.InventoryNumber == "EH-302" && m.IsOperational);

                if (activeSite != null && excavator != null && dumpTruck != null)
                {
                    var logs = new List<DailyWorkLog>
                    {
                        new DailyWorkLog
                        {
                            LogDate = new DateTime(2026, 11, 12),
                            WorkSiteId = activeSite.Id,
                            HeavyMachineId = excavator.Id,
                            OperatorName = employeeEmail,
                            HoursWorked = 8.5,
                            FuelConsumedLiters = 162.0,
                            DebrisHauledTons = 0.0,
                            Notes = "Primary concrete wall demolition and rebar separation."
                        },
                        new DailyWorkLog
                        {
                            LogDate = new DateTime(2026, 11, 13),
                            WorkSiteId = activeSite.Id,
                            HeavyMachineId = dumpTruck.Id,
                            OperatorName = adminEmail,
                            HoursWorked = 8.0,
                            FuelConsumedLiters = 135.5,
                            DebrisHauledTons = 148.0,
                            Notes = "6 full loads of crushed concrete hauled to recycling depot."
                        }
                    };

                    await context.DailyWorkLogs.AddRangeAsync(logs);

                    // Add the newly recorded working hours to the machines' existing totals.
                    excavator.CurrentEngineHours += logs
                        .Where(l => l.HeavyMachineId == excavator.Id)
                        .Sum(l => l.HoursWorked);

                    dumpTruck.CurrentEngineHours += logs
                        .Where(l => l.HeavyMachineId == dumpTruck.Id)
                        .Sum(l => l.HoursWorked);

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}