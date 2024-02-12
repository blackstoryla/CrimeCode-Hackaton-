namespace CrimeCode.Models
{
    public class TwoString
    {
        public int Id { get; set; }
        public string String1 { get; set; }
        public string String2 { get; set; }

        public TwoString(int id, string string1, string string2)
        {
            Id = id;
            String1 = string1;
            String2 = string2;
        }
    }
}
