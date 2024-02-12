using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace CrimeCode.Models
{
    public class TaskTag
    {
        [Key]
        public int Id { get; set; }
        public int IdTask { get; set; }
        public int IdTag { get; set; }

        public TaskTag() { }
    }
}
