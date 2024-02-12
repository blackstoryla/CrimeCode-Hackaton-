using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrimeCode.Models
{
    public class ProjectTask
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StoryPoint { get; set; }
        public int Priority { get; set;}

        public ProjectTask() { }

        public ProjectTask(string name, string description, int storyPoint, int priority)
        {
            Name = name;
            Description = description;
            StoryPoint = storyPoint;
            Priority = priority;
        }
    }
}
