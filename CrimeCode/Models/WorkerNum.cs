namespace CrimeCode.Models
{
    public class WorkerNum
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int TaskNum { get; set; }

        public WorkerNum() { }
        public WorkerNum(int id, string name, int level, int taskNum)
        {
            Id = id;
            Name = name;
            Level = level;
            TaskNum = taskNum;
        }
    }
}
