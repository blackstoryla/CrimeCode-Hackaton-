using CrimeCode.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using CrimeCode.Data;
using CrimeCode.Models;
using System.Net.Sockets;

namespace CrimeCode.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<WorkerTask> workerTasks = await _context.WorkerTask.ToListAsync();
            List<TwoString> list = new List<TwoString>();

            foreach (var x in workerTasks)
            {
                if (x.IdWorker == 0)
                {
                    list.Add(new TwoString(x.Id, "-----",
                    (await _context.ProjectTask.FirstOrDefaultAsync(m => m.Id == x.IdTask)).Name));
                }
                else
                {
                    list.Add(new TwoString(x.Id, (await _context.Worker.FirstOrDefaultAsync(m => m.Id == x.IdWorker)).Name,
                        (await _context.ProjectTask.FirstOrDefaultAsync(m => m.Id == x.IdTask)).Name));
                }
            }

            return View(list);
        }

        public async Task<IActionResult> Agrigate(string storyPoint)
        {
            int StoryPoint = 0;
            try
            {
                StoryPoint = Int32.Parse(storyPoint);
            }
            catch {
                StoryPoint = 0;
            }

            List<WorkerTask> table = await _context.WorkerTask.ToListAsync(); ;

            foreach (var x in table)
            {
                _context.WorkerTask.Remove(x);
            }

            await _context.SaveChangesAsync();
       


            List<Worker> workers = await _context.Worker.ToListAsync();
            List<WorkerTag> workersTags = await _context.WorkerTag.ToListAsync();
            List<ProjectTask> tasks = await _context.ProjectTask.ToListAsync();
            List<TaskTag> taskTags = await _context.TaskTag.ToListAsync();

            List<WorkerNum> workerNums = new List<WorkerNum>();

            foreach (var x in workers)
            {
                int level = 0;
                if (x.Level == "Senior") level = 1;
                else if (x.Level == "Middle") level = 2;
                else level = 3;
                workerNums.Add(new WorkerNum(x.Id, x.Name, level, 0));
            }

            List<WorkerTask> workersTask = GetWorkerTask(taskTags, workersTags, tasks, workerNums, StoryPoint);

            foreach (var x in workersTask)
            {
                _context.Add(x);
                await _context.SaveChangesAsync();
            }

            List<WorkerTask> workerTasks = await _context.WorkerTask.ToListAsync();
            List<TwoString> list = new List<TwoString>();

            foreach (var x in workerTasks)
            {
                if (x.IdWorker == 0)
                {
                    list.Add(new TwoString(x.Id, "-----",
                    (await _context.ProjectTask.FirstOrDefaultAsync(m => m.Id == x.IdTask)).Name));
                }
                else
                {
                    list.Add(new TwoString(x.Id, (await _context.Worker.FirstOrDefaultAsync(m => m.Id == x.IdWorker)).Name,
                        (await _context.ProjectTask.FirstOrDefaultAsync(m => m.Id == x.IdTask)).Name));
                }
            }

            return View("Index",list);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: Home/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.WorkerTask == null)
            {
                return NotFound();
            }

            var projectTask = await _context.WorkerTask.FindAsync(id);
            if (projectTask == null)
            {
                return NotFound();
            }
            return View(projectTask);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, IdWorker,IdTask")] WorkerTask workerTask)
        {
            if (id != workerTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workerTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkerTaskExists(workerTask.Id))
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
            return View(workerTask);
        }

        private bool WorkerTaskExists(int id)
        {
            return (_context.WorkerTask?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        static List<ProjectTask> ChooseTasksForSprint(int sprintSP, List<ProjectTask> Tasks)
        {
            List<ProjectTask> sprintTask = new List<ProjectTask>();
            int sp = 0;
            int i = 0;
            while (sp + sprintSP * 0.1 < sprintSP && i < Tasks.Count())
            {
                sp += Tasks[i].StoryPoint;
                sprintTask.Add(Tasks[i]);
                i++;
            }

            return sprintTask;
        }


        static List<ProjectTask> TaskSort(List<ProjectTask> Tickets)
        {
            return Tickets.OrderBy(i => i.Priority).ThenByDescending(i => i.StoryPoint).ToList();
        }

        static bool CheckTags(List<TaskTag> TaskNTags, List<WorkerTag> WorkerNTags, ProjectTask Task, WorkerNum worker)
        {
            int taskTag = TaskNTags.Find(item => item.IdTask == Task.Id).IdTag;

            var workerTags = WorkerNTags.Where(x => x.IdWorker == worker.Id).ToList();

            if (workerTags.Exists(x => x.IdTag == taskTag))
                return true;
            else return false;
        }


        public static List<WorkerTask> GetWorkerTask(List<TaskTag> TaskNTags, List<WorkerTag> WorkerNTags, List<ProjectTask> Tickets, List<WorkerNum> Workers, int sprintSP)
        {
            Tickets = TaskSort(Tickets);
            List<ProjectTask> sprintTask = ChooseTasksForSprint(sprintSP, Tickets);
            List<WorkerTask> WorkerNTasks = new List<WorkerTask>();

            foreach (var task in sprintTask)
            {

                List<WorkerNum> WorkersForTask = new List<WorkerNum>();
                try
                {
                    foreach (var worker in Workers)
                    {
                        if (CheckTags(TaskNTags, WorkerNTags, task, worker))
                            WorkersForTask.Add(worker);
                    }

                    WorkersForTask.OrderBy(i => i.Level).ThenBy(i => i.TaskNum).ToList();  //затем сортируем по количеству задач
                    int workerid;

                    if (task.StoryPoint > 4)
                    {
                        //найди первого сениора (без задач)
                        workerid = WorkersForTask.Find(item => item.Level == 1).Id;

                    }
                    else if (task.StoryPoint > 2)
                    {
                        //найди первого миддла (без задач)
                        workerid = WorkersForTask.Find(item => item.Level <= 2).Id;

                    }
                    else
                    {
                        //найди первого джуна (без задач)
                        workerid = WorkersForTask.Find(item => item.Level <= 3).Id;

                    }
                    WorkerTask workerNTask = new WorkerTask(workerid, task.Id);
                    int idToChange = Workers.FindIndex(item => item.Id == workerid);

                    int workerId = Workers[idToChange].Id;
                    string name = Workers[idToChange].Name;
                    int grade = Workers[idToChange].Level;
                    int taskNum = Workers[idToChange].TaskNum;

                    Workers.RemoveAt(idToChange);
                    Workers.Add(new WorkerNum(workerId, name, grade, ++taskNum));

                    WorkerNTasks.Add(workerNTask);
                }
                catch (Exception ex)
                {
                    WorkerTask workerNTask = new WorkerTask(0, task.Id);
                    WorkerNTasks.Add(workerNTask);
                }
            }

            return WorkerNTasks;
        }
    }
}