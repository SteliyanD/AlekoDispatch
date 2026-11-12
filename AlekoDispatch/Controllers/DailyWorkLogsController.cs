using AlekoDispatch.Services;
using AlekoDispatch.ViewModels.DailyWorkLogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlekoDispatch.Controllers
{
    [Authorize]
    public class DailyWorkLogsController : Controller
    {
        private readonly IDailyWorkLogService _logService;

        public DailyWorkLogsController(IDailyWorkLogService logService)
        {
            _logService = logService;
        }

        // GET: /DailyWorkLogs
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var logs = await _logService.GetAllAsync();
            return View(logs);
        }

        // GET: /DailyWorkLogs/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var defaultOperator = User.Identity?.Name;
            var model = await _logService.GetNewLogFormModelAsync(defaultOperator);
            return View(model);
        }

        // POST: /DailyWorkLogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DailyWorkLogFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableWorkSites = await _logService.GetWorkSitesSelectListAsync();
                model.AvailableMachines = await _logService.GetOperationalMachinesSelectListAsync();
                return View(model);
            }

            await _logService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // POST: /DailyWorkLogs/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _logService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}