using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskManegementSystem.Areas.Identity.Data;
using TaskManegementSystem.Data;
using TaskManegementSystem.Models;

namespace TaskManegementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TaskManegementDbContext _db;
        private readonly UserManager<TaskManegementSystemUser> _userManager;
        public HomeController(ILogger<HomeController> logger, TaskManegementDbContext db , UserManager<TaskManegementSystemUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> IndexAsync()
        {

            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                string firstName = user.FirstName;
                var tasks = _db.Tasks.Where(task => task.AssignedUser == firstName).ToList();
                return View(tasks);

            }
           
            

            return View();
        }

       
        [Authorize]
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