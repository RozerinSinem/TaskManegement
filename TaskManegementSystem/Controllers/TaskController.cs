using Microsoft.AspNetCore.Mvc;
using TaskManegementSystem.Data;
using TaskManegementSystem.Models;

namespace TaskManegementSystem.Controllers

{
    public class TaskController : Controller
    {
        private readonly TaskManegementDbContext _db;

        public TaskController (TaskManegementDbContext db)
        {
            _db = db;
        }


        [HttpGet]
        public IActionResult CreateTask()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTask(TaskViewModel obj)
        {

            if(ModelState.IsValid)
            {

                
                _db.Tasks.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Success");
            }

            return View(obj);
        }

        public IActionResult Success()
        {

          return View();
        }

    }
}
