using AlekoDispatch.Data.Enums;
using AlekoDispatch.Services;
using AlekoDispatch.ViewModels.HeavyMachines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlekoDispatch.Controllers
{
    [Authorize]
    public class HeavyMachinesController : Controller
    {
        private readonly IHeavyMachineService _machineService;

        public HeavyMachinesController(IHeavyMachineService machineService)
        {
            _machineService = machineService;
        }

        // GET: /HeavyMachines
        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] string? searchTerm,
            [FromQuery] MachineType? category,
            [FromQuery] bool? isOperational)
        {
            var model = await _machineService.GetAllAsync(searchTerm, category, isOperational);
            return View(model);
        }

        // GET: /HeavyMachines/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var machine = await _machineService.GetDetailsByIdAsync(id);
            if (machine == null)
            {
                return NotFound();
            }

            return View(machine);
        }

        // GET: /HeavyMachines/Create
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create()
        {
            var model = await _machineService.GetNewMachineFormModelAsync();
            return View(model);
        }

        // POST: /HeavyMachines/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(HeavyMachineFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableWorkSites = await _machineService.GetWorkSitesSelectListAsync();
                return View(model);
            }

            await _machineService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: /HeavyMachines/Edit/{id}
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _machineService.GetForEditByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: /HeavyMachines/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, HeavyMachineFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableWorkSites = await _machineService.GetWorkSitesSelectListAsync();
                return View(model);
            }

            var success = await _machineService.EditAsync(id, model);
            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /HeavyMachines/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _machineService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}