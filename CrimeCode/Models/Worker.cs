using System.ComponentModel.DataAnnotations;

namespace CrimeCode.Models
{
    public class Worker
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }

        public Worker() { }
    }
}
