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
    public class WorkerTagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkerTagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WorkerTags
        public async Task<IActionResult> Index()
        {
            List<WorkerTag> tasks = await _context.WorkerTag.ToListAsync();

            List<TwoString> list = new List<TwoString>();

            foreach(var x in tasks)
            {
                list.Add(new TwoString(x.Id, (await _context.Worker.FirstOrDefaultAsync(m => m.Id == x.IdWorker)).Name,
                    (await _context.Tag.FirstOrDefaultAsync(m => m.Id == x.IdTag)).Name));
            }

            return _context.WorkerTag != null ? 
                          View(list) :
                          Problem("Entity set 'ApplicationDbContext.WorkerTag'  is null.");
        }

        // GET: WorkerTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.WorkerTag == null)
            {
                return NotFound();
            }

            var workerTag = await _context.WorkerTag
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workerTag == null)
            {
                return NotFound();
            }

            return View(workerTag);
        }

        // GET: WorkerTags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WorkerTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdWorker,IdTag")] WorkerTag workerTag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workerTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workerTag);
        }

        // GET: WorkerTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.WorkerTag == null)
            {
                return NotFound();
            }

            var workerTag = await _context.WorkerTag.FindAsync(id);
            if (workerTag == null)
            {
                return NotFound();
            }
            return View(workerTag);
        }

        // POST: WorkerTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdWorker,IdTag")] WorkerTag workerTag)
        {
            if (id != workerTag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workerTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkerTagExists(workerTag.Id))
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
            return View(workerTag);
        }

        // GET: WorkerTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.WorkerTag == null)
            {
                return NotFound();
            }

            var workerTag = await _context.WorkerTag
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workerTag == null)
            {
                return NotFound();
            }

            return View(workerTag);
        }

        // POST: WorkerTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.WorkerTag == null)
            {
                return Problem("Entity set 'ApplicationDbContext.WorkerTag'  is null.");
            }
            var workerTag = await _context.WorkerTag.FindAsync(id);
            if (workerTag != null)
            {
                _context.WorkerTag.Remove(workerTag);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkerTagExists(int id)
        {
          return (_context.WorkerTag?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
