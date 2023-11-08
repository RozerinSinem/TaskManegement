using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManegementSystem.Areas.Identity.Data;
using TaskManegementSystem.Data;
using TaskManegementSystem.Models;

namespace TaskManegementSystem.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskManegementDbContext _context;

        private readonly UserManager<TaskManegementSystemUser> _userManager;

        public TaskController(TaskManegementDbContext context, UserManager<TaskManegementSystemUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
              return _context.Tasks != null ? 
                          View(await _context.Tasks.ToListAsync()) :
                          Problem("Entity set 'TaskManegementDbContext.Tasks'  is null.");
        }

       
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var taskViewModel = await _context.Tasks
                .FirstOrDefaultAsync(m => m.TaskID == id);
            if (taskViewModel == null)
            {
                return NotFound();
            }

            return View(taskViewModel);
        }
        [Authorize]
        [HttpGet]
        public IActionResult CreateTask()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTask(TaskViewModel obj)
        {

            if (ModelState.IsValid)
            {


                _context.Tasks.Add(obj);
                _context.SaveChanges();
                return RedirectToAction("Success");
            }

            return View(obj);
        }

        public IActionResult Success()
        {

            return View();
        }
    
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskID,Name,Description,AssignedUser")] TaskViewModel taskViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskViewModel);
        }

       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var taskViewModel = await _context.Tasks.FindAsync(id);
            if (taskViewModel == null)
            {
                return NotFound();
            }
            return View(taskViewModel);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskID,Name,Description,AssignedUser")] TaskViewModel taskViewModel)
        {
            if (id != taskViewModel.TaskID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskViewModelExists(taskViewModel.TaskID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taskViewModel);
        }

      
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var taskViewModel = await _context.Tasks
                .FirstOrDefaultAsync(m => m.TaskID == id);
            if (taskViewModel == null)
            {
                return NotFound();
            }

            return View(taskViewModel);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tasks == null)
            {
                return Problem("Entity set 'TaskManegementDbContext.Tasks'  is null.");
            }
            var taskViewModel = await _context.Tasks.FindAsync(id);
            if (taskViewModel != null)
            {
                _context.Tasks.Remove(taskViewModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskViewModelExists(int id)
        {
          return (_context.Tasks?.Any(e => e.TaskID == id)).GetValueOrDefault();
        }
        [Authorize]
        public async Task<IActionResult> AssignTaskAsync()
        {

            var tasks = _context.Tasks.Where(task => task.AssignedUser == null).ToList();

            var users = await _userManager.Users.ToListAsync();

            List<string> userNames = users.Select(user => user.FirstName).ToList();

            ViewBag.UserNames = new SelectList(userNames);

            return View(tasks);



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignTask(int taskId, string AssignedUser)
        {
            var task = await _context.Tasks.FindAsync(taskId);

            if (task == null)
            {
                return NotFound();
            }

            task.AssignedUser = AssignedUser;

            try
            {
                _context.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskViewModelExists(task.TaskID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
