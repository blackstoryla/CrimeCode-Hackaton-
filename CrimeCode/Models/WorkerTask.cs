using System.ComponentModel.DataAnnotations;

namespace CrimeCode.Models
{
    public class WorkerTask
    {
        [Key]
        public int Id { get; set; }
        public int IdWorker { get; set; }
        public int IdTask { get; set; }

        public WorkerTask() { }
        public WorkerTask( int idWorker, int idTask)
        {
            IdWorker = idWorker;
            IdTask = idTask;
        }
    }
}
