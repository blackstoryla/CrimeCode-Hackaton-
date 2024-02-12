using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrimeCode.Data;
using CrimeCode.Models;

namespace CrimeCode.Controllers
{
    public class TaskTagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskTagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TaskTags
        public async Task<IActionResult> Index()
        {
            List<TaskTag> tasks = await _context.TaskTag.ToListAsync();

            List<TwoString> list = new List<TwoString>();

            foreach (var x in tasks)
            {
                list.Add(new TwoString(x.Id, (await _context.ProjectTask.FirstOrDefaultAsync(m => m.Id == x.IdTask)).Name,
                    (await _context.Tag.FirstOrDefaultAsync(m => m.Id == x.IdTag)).Name));
            }

            return _context.WorkerTag != null ?
                          View(list) :
                          Problem("Entity set 'ApplicationDbContext.ProjectTaskTag'  is null.");
        }

        // GET: TaskTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TaskTag == null)
            {
                return NotFound();
            }

            var taskTag = await _context.TaskTag
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskTag == null)
            {
                return NotFound();
            }

            return View(taskTag);
        }

        // GET: TaskTags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaskTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdTask,IdTag")] TaskTag taskTag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskTag);
        }


        // GET: TaskTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TaskTag == null)
            {
                return NotFound();
            }

            var taskTag = await _context.TaskTag.FindAsync(id);
            if (taskTag == null)
            {
                return NotFound();
            }
            return View(taskTag);
        }

        // POST: TaskTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdTask,IdTag")] TaskTag taskTag)
        {
            if (id != taskTag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskTagExists(taskTag.Id))
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
            return View(taskTag);
        }

        // GET: TaskTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TaskTag == null)
            {
                return NotFound();
            }

            var taskTag = await _context.TaskTag
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskTag == null)
            {
                return NotFound();
            }

            return View(taskTag);
        }

        // POST: TaskTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TaskTag == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TaskTag'  is null.");
            }
            var taskTag = await _context.TaskTag.FindAsync(id);
            if (taskTag != null)
            {
                _context.TaskTag.Remove(taskTag);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskTagExists(int id)
        {
          return (_context.TaskTag?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
