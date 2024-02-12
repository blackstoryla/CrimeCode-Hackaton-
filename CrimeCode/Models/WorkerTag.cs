using System.ComponentModel.DataAnnotations;

namespace CrimeCode.Models
{
    public class WorkerTag
    {
        [Key]
        public int Id { get; set; }
        public int IdWorker { get; set; }
        public int IdTag { get; set; }

        public WorkerTag() { }
    }
}
