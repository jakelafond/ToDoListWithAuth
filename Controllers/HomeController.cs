using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todolistwithauth.Data;
using todolistwithauth.Models;

namespace todolistwithauth.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> um)
        {
            _context = context;
            _userManager = um;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var applicationDbContext = _context.Todo.Include(p => p.User).Where(x => x.UserId == user.Id);

            return View(applicationDbContext.ToList());

        }

        [HttpPost]
        public async Task<IActionResult> Create(string newToDo)
        {
            var todo = new ToDoModel();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            todo.UserId = user.Id;
            todo.Description = newToDo;
            _context.Add(todo);
            await _context.SaveChangesAsync();
            var applicationDbContext = _context.Todo.Include(p => p.User).Where(x => x.UserId == user.Id);
            return View("Index", await applicationDbContext.ToListAsync());
        }
        public IActionResult Add(string newToDo)
        {
            var toDo = new ToDoModel
            {
                Description = newToDo
            };
            _context.Todo.Add(toDo);
            _context.SaveChanges();
            return View("Index", _context.Todo.ToList());
        }

        public IActionResult Complete(int id)
        {
            var toDo = _context.Todo.FirstOrDefault(i => i.ID == id);
            toDo.CompleteTask();
            _context.SaveChanges();
            return Redirect("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
