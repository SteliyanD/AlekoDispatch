using AlekoDispatch.Data.Enums;
using AlekoDispatch.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace AlekoDispatch.Data
{
    public class DbInitializer
    {
        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

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
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
            if (existingAdmin == null)
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

            // WorkSites seeding with static dates
            if (!context.WorkSites.Any())
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
                    }
                };

                await context.WorkSites.AddRangeAsync(workSites);
                await context.SaveChangesAsync();
            }

            // HeavyMachines seeding
            if (!context.HeavyMachines.Any())
            {
                var firstSite = context.WorkSites.FirstOrDefault();

                var machines = new List<HeavyMachine>
                {
                    new HeavyMachine
                    {
                        Model = "CAT 320 GC",
                        InventoryNumber = "EH-104",
                        Type = MachineType.Excavator,
                        CurrentEngineHours = 3450.5,
                        IsOperational = true,
                        CurrentWorkSiteId = firstSite?.Id
                    },
                    new HeavyMachine
                    {
                        Model = "Komatsu PC210LC",
                        InventoryNumber = "EH-105",
                        Type = MachineType.Excavator,
                        CurrentEngineHours = 5120.0,
                        IsOperational = true,
                        CurrentWorkSiteId = firstSite?.Id
                    },
                    new HeavyMachine
                    {
                        Model = "JCB 4CX",
                        InventoryNumber = "EH-201",
                        Type = MachineType.WheelLoader,
                        CurrentEngineHours = 2100.8,
                        IsOperational = true,
                        CurrentWorkSiteId = null
                    },
                    new HeavyMachine
                    {
                        Model = "Mercedes-Benz Arocs 4145 (4-Axle)",
                        InventoryNumber = "EH-302",
                        Type = MachineType.DumpTruck,
                        CurrentEngineHours = 6800.2,
                        IsOperational = true,
                        CurrentWorkSiteId = firstSite?.Id
                    }
                };

                await context.HeavyMachines.AddRangeAsync(machines);
                await context.SaveChangesAsync();
            }
        }
    }
}
