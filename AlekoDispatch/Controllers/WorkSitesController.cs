using AlekoDispatch.Services;
using AlekoDispatch.ViewModels.WorkSites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlekoDispatch.Controllers
{
    [Authorize]
    public class WorkSitesController : Controller
    {
        private readonly IWorkSiteService _workSiteService;

        public WorkSitesController(IWorkSiteService workSiteService)
        {
            _workSiteService = workSiteService;
        }

        // GET: /WorkSites
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _workSiteService.GetAllAsync();
            return View(model);
        }

        // GET: /WorkSites/Details/{id} 
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _workSiteService.GetDetailsByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: /WorkSites/Create
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            var model = new WorkSiteFormModel();
            return View(model);
        }

        // POST: /WorkSites/Create
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkSiteFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _workSiteService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: /WorkSites/Edit/{id}
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _workSiteService.GetForEditByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: /WorkSites/Edit/{id}
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WorkSiteFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await _workSiteService.EditAsync(id, model);
            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /WorkSites/Delete/{id}
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _workSiteService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
