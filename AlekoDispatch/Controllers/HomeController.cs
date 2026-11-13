using AlekoDispatch.Data.Models;
using AlekoDispatch.Services;
using AlekoDispatch.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AlekoDispatch.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var dashboardModel = await _dashboardService.GetDashboardStatsAsync();
                return View(dashboardModel);
            }

            return View(null);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}