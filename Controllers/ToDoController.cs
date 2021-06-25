using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Infrastructure;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ToDoContext context;

        public ToDoController(ToDoContext context)
        {
            this.context = context;
        }


        // Get
        public async Task<IActionResult> Index()
        {
            IQueryable<TodoList> items = from i in context.ToDoList orderby i.Id select i;

            List<TodoList> todoList = await items.ToListAsync();

            return View(todoList);
        }


        // Get /ToDo/create
        public IActionResult Create() => View();

        // Post /ToDo/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoList item)
        {
            if (ModelState.IsValid)
            {
                context.Add(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "The item has been added!";

                return RedirectToAction("Index");

            }

            return View(item);
        }

        // Get /ToDo/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            TodoList item = await context.ToDoList.FindAsync(id);

            if(item == null)
            {
                return NotFound();
            }

            return View(item);
        }


        // Post /ToDo/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TodoList item)
        {
            if (ModelState.IsValid)
            {
                context.Update(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "The item has been updated!";

                return RedirectToAction("Index");

            }

            return View(item);
        }

        // Get /ToDo/delete/5
        public async Task<IActionResult> Delete(int id)
        {
            TodoList item = await context.ToDoList.FindAsync(id);

            if (item == null)
            {
                TempData["Error"] = "The item does not exist!";
            }else
            {
                context.ToDoList.Remove(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "The item has been deleted!";
            }

            return RedirectToAction("Index");
        }
    }
}
